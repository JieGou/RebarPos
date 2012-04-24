﻿using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using OZOZ.RebarPosWrapper;


// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(RebarPosCommands.RebarPosCommands))]

namespace RebarPosCommands
{
    // This class is instantiated by AutoCAD for each document when
    // a command is called by the user the first time in the context
    // of a given document. In other words, non static data in this class
    // is implicitly per-document!
    public partial class RebarPosCommands
    {
        // The CommandMethod attribute can be applied to any public  member 
        // function of any public class.
        // The function should take no arguments and return nothing.
        // If the method is an instance member then the enclosing class is 
        // instantiated for each document. If the member is a static member then
        // the enclosing class is NOT instantiated.
        //
        // NOTE: CommandMethod has overloads where you can provide helpid and
        // context menu.
        public RebarPosCommands()
        {
            DWGUtility.CreateDefaultShapes();
            DWGUtility.CreateDefaultStyles();
            DWGUtility.CreateDefaultGroups();
        }

        private string CurrentGroup { get; set; }

        [CommandMethod("RebarPos", "POS", "POS_Local", CommandFlags.Modal)]
        public void CMD_Pos()
        {
            bool cont = true;
            while (cont)
            {
                // AcString group = DWGUtility::ReadSettings().CurrentGroup;
                PromptEntityOptions opts = new PromptEntityOptions("Poz secin veya [Yeni/Numaralandir/Kopyala/Grup (" + CurrentGroup + ")/Metraj/bul Degistir/Balon sil/Cizim/Secenekler/Acilimlar/Poz stili]: ",
                    "New Numbering Copy Group BOM Find Empty Draw Options Shapes Pos");
                PromptEntityResult result = Application.DocumentManager.MdiActiveDocument.Editor.GetEntity(opts);

                if (result.Status == PromptStatus.Keyword)
                {
                    switch (result.StringResult)
                    {
                        case "New":
                            NewPos();
                            break;
                        case "Numbering":
                            // NumberPos();
                            break;
                        case "Copy":
                            // CopyPos();
                            break;
                        case "Group":
                            // PosGroup();
                            break;
                        case "BOM":
                            // DrawBOM();
                            break;
                        case "Find":
                            // FindPos();
                            break;
                        case "Empty":
                            EmptyBalloons();
                            break;
                        case "Draw":
                            // DrawPos();
                            break;
                        case "Options":
                            // PosOptions();
                            break;
                        case "Shapes":
                            // PosShapes();
                            break;
                        case "Pos":
                            // PosStyle();
                            break;
                    }
                }
                else if (result.Status == PromptStatus.OK)
                {
                    bool success = PosEdit(result.ObjectId, result.PickedPoint);
                    cont = !success;
                }
                else
                {
                    cont = false;
                }
            }
        }

        [CommandMethod("RebarPos", "POSEDIT", "POSEDIT_Local", CommandFlags.Modal | CommandFlags.UsePickSet)]
        public void CMD_PosEdit()
        {
            PromptSelectionOptions opts = new PromptSelectionOptions();
            opts.SingleOnly = true;
            PromptSelectionResult result = Application.DocumentManager.MdiActiveDocument.Editor.GetSelection(opts);
            if (result.Status == PromptStatus.OK && result.Value.Count == 1)
            {
                PosEdit(result.Value[0].ObjectId, Point3d.Origin);
            }
        }

        [CommandMethod("RebarPos", "NEWPOS", "NEWPOS_Local", CommandFlags.Modal)]
        public void CMD_NewPos()
        {
            NewPos();
        }

        [CommandMethod("RebarPos", "EMPTYPOS", "EMPTYPOS_Local", CommandFlags.Modal)]
        public void CMD_EmptyBalloons()
        {
            EmptyBalloons();
        }
    }
}