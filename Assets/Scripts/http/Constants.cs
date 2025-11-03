public static class Constants
{
    // Production API URL
    public static readonly string PRODUCTION_API_URL = "https://anato-learn-api.vercel.app";

    // public static readonly string PRODUCTION_API_URL = "https://anatolearn-api.onrender.com";

    // Development API URL (for local testing)
    public static readonly string DEVELOPMENT_API_URL = "http://localhost:8000";

    // Use this to switch between development and productioan
    public static readonly bool IS_DEVELOPMENT = false; // Set to false for production builds

    // Current API URL based on environment
    // public static string API_URL = DEVELOPMENT_API_URL;
    public static string API_URL => IS_DEVELOPMENT ? DEVELOPMENT_API_URL : PRODUCTION_API_URL;
}
