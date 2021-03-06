﻿using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace Bowerbird
{
    public class Info : GH_AssemblyInfo
    {
        public override string Name => "Bowerbird2";

        public override Bitmap Icon => Properties.Resources.logo_24;

        public override string Description => "";

        public override Guid Id => new Guid("6ad6b4dc-e6e6-40a4-9198-3a2c55b9ad30");

        public override string AuthorName => "Thomas Oberbichler";

        public override string AuthorContact => "thomas.oberbichler@gmail.com";

        public override string Version => "2.1.4";

        public override string AssemblyVersion => this.Version;
    }
}
