using System;

public enum Experimental
{
    NONE = -1,
    FRIEND_SYSTEM,
    IMPROVEMENT_DESIGN,
    [Obsolete("정식 편입으로 실험적 기능에서 삭제")]
    QUEST
}
