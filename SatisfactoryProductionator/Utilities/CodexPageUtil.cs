namespace SatisfactoryProductionator.Utilities
{
    public static class CodexPageUtil
    {
        public static string GetMarginCss(int cnt) => cnt == 1 ? "mt-48" : "mt-24";

        public static string GetImageSizeCss(int cnt) => cnt == 1 ? "image-128" : "image-96";
    }
}
