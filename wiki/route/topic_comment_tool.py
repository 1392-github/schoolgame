from .tool.func import *

def topic_comment_tool(topic_num = 1, num = 1):
    with get_db_connect() as conn:
        curs = conn.cursor()
        
        num = str(num)
        topic_num = str(topic_num)
        
        if acl_check(conn, '', 'topic_view', topic_num) == 1:
            return re_error(conn, '/ban')

        curs.execute(db_change("select block, ip, date from topic where code = ? and id = ?"), [topic_num, num])
        data = curs.fetchall()
        if not data:
            return redirect(conn, '/thread/' + topic_num)

        ban = '''
            <h2>''' + get_lang(conn, 'state') + '''</h2>
            <ul class="opennamu_ul">
                <li>''' + get_lang(conn, 'writer') + ' : ''' + ip_pas(conn, data[0][1]) + '''</li>
                <li>''' + get_lang(conn, 'time') + ' : ' + data[0][2] + '''</li>
            </ul>
            <h2>''' + get_lang(conn, 'other_tool') + '''</h2>
            <ul class="opennamu_ul">
                <li>
                    <a href="/thread/''' + topic_num + '/comment/' + num + '''/raw">''' + get_lang(conn, 'raw') + '''</a>
                </li>
            </ul>
        '''

        if admin_check(conn, 3) == 1:
            ban += '''
                <h2>''' + get_lang(conn, 'admin_tool') + '''</h2>
                <ul class="opennamu_ul">
                    <li>
                        <a href="/auth/give/ban/''' + url_pas(data[0][1]) + '''">
                            ''' + (get_lang(conn, 'ban') + ' | ' + get_lang(conn, 'release')) + '''
                        </a>
                    </li>
                    <li>
                        <a href="/thread/''' + topic_num + '''/comment/''' + num + '''/blind">
                            ''' + (get_lang(conn, 'hide') + ' | ' + get_lang(conn, 'hide_release')) + '''
                        </a>
                    </li>
                    <li>
                        <a href="/thread/''' + topic_num + '''/comment/''' + num + '''/notice">
                            ''' + (get_lang(conn, 'pinned') + ' | ' + get_lang(conn, 'pinned_release')) + '''
                        </a>
                    </li>
                    <li>
                        <a href="/thread/''' + topic_num + '''/comment/''' + num + '''/delete">
                            ''' + get_lang(conn, 'delete') + '''
                        </a>
                </ul>
            '''

        return easy_minify(conn, flask.render_template(skin_check(conn),
            imp = [get_lang(conn, 'discussion_tool'), wiki_set(conn), wiki_custom(conn), wiki_css(['(#' + num + ')', 0])],
            data = ban,
            menu = [['thread/' + topic_num + '#' + num, get_lang(conn, 'return')]]
        ))