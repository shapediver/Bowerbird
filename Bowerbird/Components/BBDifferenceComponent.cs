﻿using ClipperLib;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace Bowerbird.Components
{
    public class BBDifferenceComponent : GH_Component
    {
        public BBDifferenceComponent() : base("BB Difference", "BBDiff", "Difference of a set of planar closed polylines" + Util.InfoString, "Bowerbird", "Polyline") { }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "A", "", GH_ParamAccess.list);
            pManager.AddCurveParameter("Curve", "B", "", GH_ParamAccess.list);
            pManager.AddPlaneParameter("Plane", "P", "", GH_ParamAccess.item);

            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // --- Input

            var curvesA = new List<Curve>();
            var curvesB = new List<Curve>();
            var plane = default(Plane?);

            DA.GetDataList(0, curvesA);
            DA.GetDataList(1, curvesB);
            DA.GetData(2, ref plane);


            // --- Execute

            var result = BBPolyline.Boolean(ClipType.ctDifference, PolyFillType.pftNonZero, curvesA, curvesB, plane);
            

            // --- Output

            DA.SetDataList(0, result);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get { return Properties.Resources.icon_difference; }
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{EA7CEFD1-6123-453E-9A8D-5F2D1CE3610A}"); }
        }
    }
}
