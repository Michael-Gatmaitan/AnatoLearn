public class PagesMap
{
    private PagesMap(string value)
    {
        Value = value;
    }

    public string Value { get; private set; }

    public static PagesMap LoginPage
    {
        get { return new PagesMap("LoginPage"); }
    }
    public static PagesMap SignupPage
    {
        get { return new PagesMap("SignupPage"); }
    }
    public static PagesMap HomePage
    {
        get { return new PagesMap("HomePage"); }
    }
    public static PagesMap PopupPage
    {
        get { return new PagesMap("PopupPage"); }
    }
    public static PagesMap QuizPage
    {
        get { return new PagesMap("QuizPage"); }
    }

    public override string ToString()
    {
        return Value;
    }
}
