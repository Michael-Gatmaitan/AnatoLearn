using System.Collections.Generic;

public class Topics
{
    public int id;
    public string topic_name;
}

public class Users
{
    public int id;
    public string name;
    public string email;
    public string fname;
    public string mname;
    public string lname;
}

// Activities
public class Activities
{
    public int id;
    public int topic_id;
    public int act_type_id;
}

public class Act_QA
{
    public int id;
    public int act_id;
    public string question;
    public string answer;
    public string choices;
}

public class QA_Choices
{
    public string a;
    public string b;
    public string c;
    public string d;
}

public class Activity_Type
{
    public int id;
    public string act_type; // tap, mcq, tof
}

// Scores
public class Activity_Scores
{
    public int id;
    public int act_type_id;
    public int user_id;
    public int topic_id;
    public int score;
}

// Videos
public class Videos
{
    public int id;
    public int topic_id;
    public int video_type_id;
    public string video_path;
}

public class Video_Type
{
    public int id;
    public string video_type;
}

// Other
public class Certificates
{
    public int id;
    public int total_scores_id;
    public string image_cert_path;
    public string video_type;
    public bool has_certificate;
}
