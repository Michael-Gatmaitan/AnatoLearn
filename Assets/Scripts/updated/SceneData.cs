public static class SceneData
{
    public static bool showScorePage = false;
    public static bool showProgressionPage = false;

    public static bool studyingSkeletal = false; //edited 6-20
    public static bool studyingIntegumentary = false; //edited 6-20
    public static bool studyingDigestive = false; //edited 6-20
    public static bool studyingRespiratory = false; //edited 6-20
    public static bool studyingNervous = false; //edited 6-20
    public static bool studyingCirculatory = false; //edited 6-20
    public static bool studyingExcretory = false; //edited 6-20
    public static bool studyingCirculatoryHeart = false; //edited 6-20

    //flags for TAP ME ACT
    public static bool showTapActPage = false; // triggger to be true, when the quiz button in progression page is clicked
    public static bool exitInstrucTapMeActPageBtnIsClicked = false; //this a switch for running the Update() TapMeActManager.cs , because it might run as well on the

    public static bool is3dModelOpen = false;

    //  Explore 3d Mode. It might count scores when the  user taps the tagslabel, and detected
    //  that the clicked labelID is not equal to the question
    public static bool quizBtnIsClicked = false; //this a switch for running the TapMeActManager.cs , so that the startmethod willlnot run as long as the quizBtn is not clicked

    // public static bool takingSkeletalTapMe = false; //let's say it's true, this will be true when the quiz button in progression page is clicked for Skeletal Topic
    // public static bool takingDigestiveTapMe = false;
    // public static bool takingRespiratoryTapMe = false;
    // public static bool takingNervousTapMe = false;
    // public static bool takingIntegumentaryTapMe = false;
    // public static bool takingCirculatoryTapMe  = false;
    // public static bool takingCirculatoryHeartTapMe  = false;


    //TAGALOG-ENGLISH VERSION FLAGS
    public static string LanguageVersion = "englishVersion"; //added 7 24

    public static void resetAllFlags()
    {
        studyingSkeletal = false;
        studyingIntegumentary = false;
        studyingDigestive = false;
        studyingRespiratory = false;
        studyingCirculatory = false;
        studyingCirculatoryHeart = false;
        studyingNervous = false;
        studyingExcretory = false;
    }
}
