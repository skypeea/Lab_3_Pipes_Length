using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_3_Pipes_Length
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> elementRefList = uidoc.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element, "Выберите трубы");
            var pipeList = new List<Pipe>();
            double totalLength = 0;

            foreach (var selectedElement in elementRefList)
            {
                Element element = doc.GetElement(selectedElement);
                if (element is Pipe)
                {
                    Pipe oPipe = (Pipe)element;
                    Parameter length = oPipe.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                    totalLength += length.AsDouble();
                }
            }
            totalLength = UnitUtils.ConvertFromInternalUnits(totalLength, UnitTypeId.Meters);
            TaskDialog.Show("Длина труб", $"Длина выбранных труб: {totalLength} м.п.");


            return Result.Succeeded;

        }
    }
}
