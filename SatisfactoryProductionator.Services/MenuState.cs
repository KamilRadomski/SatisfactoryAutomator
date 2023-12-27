using SatisfactoryProductionator.DataModels.Enums;

namespace SatisfactoryProductionator.Services
{
    public class MenuState
    {
        public static Page CurrentPage { get; set; } = Page.Home;
        public static CodexCategory SelectedCategory { get; set; } = CodexCategory.Item;

        public static InfrastructureSubCategory SelectedInfrastructure { get; set; } = InfrastructureSubCategory.Foundations;

        public static UnlockSubCategory SelectedUnlock { get; set; } = UnlockSubCategory.Milestone;

        public static Dictionary<string, List<string>> ButtonConfig { get; set; } = new Dictionary<string, List<string>>();

        public event Action OnStateChange;

        private void NotifyStateChanged() => OnStateChange?.Invoke();

        #region General 
        public void SetPage(Page page)
        {
            if(page == CurrentPage) return;

            CurrentPage = page;

            NotifyStateChanged();
        }

        public Page GetPage()
        {
            return CurrentPage;
        }

        #endregion

        #region Codex

        public void SetCategory(CodexCategory category)
        {
            SelectedCategory = category;

            NotifyStateChanged();
        }

        public void SetInfrastructure(InfrastructureSubCategory category)
        {
            SelectedInfrastructure = category;

            NotifyStateChanged();
        }

        public void SetUnlock(UnlockSubCategory category)
        {
            SelectedUnlock = category;

            NotifyStateChanged();
        }

        #endregion

        #region Permutator



        #endregion

        #region Buttons

        public void SetMenuButtons(int count)
        {
            var menuKey = GetMenuKey();

            if (ButtonConfig.ContainsKey(menuKey)) return;

            ButtonConfig.Add(menuKey, new List<string>());

            var topIndex = 0;
            var bottomIndex = GetRandom();

            for (int i = 0; i < count; i++)
            {
                if (i == count - 1)
                {
                    bottomIndex = 0;
                }

                var augDefinition = GenerateAug(topIndex, bottomIndex);

                var cssClass = GenerateAugCss(topIndex, bottomIndex);

                ButtonConfig[menuKey].Add($"{augDefinition}|{cssClass}");

                topIndex = bottomIndex;
                bottomIndex = GetRandom();
            }
        }

        public string GetAugDefinition(int index)
        {
            var menuKey = GetMenuKey();

            return ButtonConfig[menuKey][index].Split('|').First();
        }

        public string GetAugCss(int index)
        {
            var menuKey = GetMenuKey();

            return ButtonConfig[menuKey][index].Split('|').Last();
        }

        private string GenerateAug(in int topIndex, in int bottomIndex)
        {
            var aug = topIndex switch
            {
                0 => "tl-clip tr-clip",
                1 or 4 => "tl-2-clip-x tr-clip",
                2 or 3 => "tl-clip tr-2-clip-x",
                5 or 6 or 7 => "tl-2-clip-x tr-2-clip-x",
                8 or 9 or 10=> "tl-clip t-clip-x tr-clip"
            };

            aug += bottomIndex switch
            {
                0 => " br-clip bl-clip",
                1 or 4 => " br-2-clip-x bl-clip",
                2 or 3 => " br-clip bl-2-clip-x",
                5 or 6 or 7 => " br-clip b-clip-x bl-clip",
                8 or 9 or 10 => " br-2-clip-x bl-2-clip-x"
            };

            return aug;
        }

        private string GenerateAugCss(in int topIndex, in int bottomIndex)
        {
            return $"t{topIndex} b{bottomIndex}";
        }

        private int GetRandom() => new Random().Next(1, 11);

        private string GetMenuKey()
        {
            if (SelectedCategory is CodexCategory.Item || SelectedCategory is CodexCategory.Building || SelectedCategory is CodexCategory.Equipment)
            {
                return SelectedCategory.ToString();
            }
            else if (SelectedCategory is CodexCategory.Infrastructure)
            {
                return SelectedInfrastructure.ToString();
            }
            else
            {
                return SelectedUnlock.ToString();
            }
        }

        #endregion

        
    }
}