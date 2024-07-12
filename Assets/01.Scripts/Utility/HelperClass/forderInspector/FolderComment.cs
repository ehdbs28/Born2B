using System.Collections.Generic;
using System;

[Serializable]
public struct FolderComment
{
    public string comment;
    public string author;  
}

[Serializable]
public class FolderCommentWrapper
{
    public List<FolderComment> comments = new List<FolderComment>();
}