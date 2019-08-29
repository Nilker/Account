using System;
using System.Collections.Generic;
using System.Text;

namespace Cig.YanFa.Navigation.Dto
{
    public class NavigationOutput
    {
        public List<NavigationInfo> firstModules { get; set; }
        public List<NavigationInfo> secondModules { get; set; }
        public string currentFirstModule { get; set; }
        public string currentSecondModule { get; set; }
        public List<NavigationInfo> threeModules { get; set; }
        public string currentThreeModule { get; set; }
    }
}
