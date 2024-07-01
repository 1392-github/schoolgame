from .tool.func import *

class api_w_render_include:
    def __init__(self, data_option):
        self.include_change_list = data_option

    def __call__(self, match):
        match_org = match.group(0)
        match = match.groups()

        if len(match) < 3:
            match = list(match) + ['']

        if match[2] == '\\':
            return match_org
        else:
            slash_add = ''
            if match[0]:
                if len(match[0]) % 2 == 1:
                    slash_add = '\\' * (len(match[0]) - 1)
                else:
                    slash_add = match[0]

            if match[1] in self.include_change_list:
                return slash_add + self.include_change_list[match[1]]
            else:
                return slash_add + match[2]

def api_w_render(db_set, name = '', tool = ''):
    with get_db_connect() as conn:
        curs = conn.cursor()

        if flask.request.method == 'POST':
            name = flask.request.form.get('name', '')
            data_org = flask.request.form.get('data', '')
            data_option = flask.request.form.get('option', '')

            markup = ''
            if tool in ('', 'from', 'include'):
                curs.execute(db_change("select set_data from data_set where doc_name = ? and set_name = 'document_markup'"), [name])
                db_data = curs.fetchall()
                if db_data and db_data[0][0] != '' and db_data[0][0] != 'normal':
                    markup = db_data[0][0]

                if markup == '':
                    curs.execute(db_change('select data from other where name = "markup"'))
                    db_data = curs.fetchall()
                    markup = db_data[0][0] if db_data else 'namumark'

            data_type = ''
            if tool == '':
                data_type = 'api_view'
            elif tool == 'from':
                data_type = 'api_from'
            elif tool == 'include':
                data_type = 'api_include'
            elif tool == 'backlink':
                data_type = 'backlink'
            else:
                data_type = 'api_thread'

            if markup in ('', 'namumark', 'namumark_beta'):
                if data_option != '':
                    data_option = json.loads(data_option)

                    data_option_func = api_w_render_include(data_option)

                    # parameter replace
                    data_org = re.sub(r'(\\+)?@([ㄱ-힣a-zA-Z0-9]+)=((?:\\@|[^@\n])+)@', data_option_func, data_org)
                    data_org = re.sub(r'(\\+)?@([ㄱ-힣a-zA-Z0-9]+)@', data_option_func, data_org)

                    # remove end br
                    data_org = re.sub('^\n+', '', data_org)

                data_pas = render_set(conn, 
                    doc_name = name, 
                    doc_data = data_org, 
                    data_type = data_type
                )

                return flask.jsonify({
                    "data" : data_pas[0], 
                    "js_data" : data_pas[1]
                })
            else:
                other_set = {}
                other_set["doc_name"] = name
                other_set["render_type"] = data_type
                other_set["data"] = data_org
                other_set = json.dumps(other_set)

                if platform.system() == 'Linux':
                    if platform.machine() in ["AMD64", "x86_64"]:
                        data = subprocess.Popen([os.path.join(".", "route_go", "bin", "main.amd64.bin"), sys._getframe().f_code.co_name, db_set, other_set], stdout = subprocess.PIPE).communicate()[0]
                    else:
                        data = subprocess.Popen([os.path.join(".", "route_go", "bin", "main.arm64.bin"), sys._getframe().f_code.co_name, db_set, other_set], stdout = subprocess.PIPE).communicate()[0]
                else:
                    if platform.machine() in ["AMD64", "x86_64"]:
                        data = subprocess.Popen([os.path.join(".", "route_go", "bin", "main.amd64.exe"), sys._getframe().f_code.co_name, db_set, other_set], stdout = subprocess.PIPE).communicate()[0]
                    else:
                        data = subprocess.Popen([os.path.join(".", "route_go", "bin", "main.arm64.exe"), sys._getframe().f_code.co_name, db_set, other_set], stdout = subprocess.PIPE).communicate()[0]

                data = data.decode('utf8')

                return flask.Response(response = data, status = 200, mimetype = 'application/json')
        else:
            return flask.jsonify({})