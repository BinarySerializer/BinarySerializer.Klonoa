using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BinarySerializer.Klonoa.DTP
{
    // The game doesn't store the cutscene text as normal encoded strings, but rather as indices to a font. The problem is that the font is
    // not global, but rather each cutscene has its own font only containing the characters used in that cutscene. Because of this the
    // indices will lead to different characters in different cutscenes. To be able to convert an index to a character we use translation
    // tables with the hash of the font image data to identify them.
    public static class CutsceneTextTranslationTables
    {
        public static Dictionary<string, char> US { get; } = new Dictionary<string, char>()
        {
            ["yNfQ7w7t+oLS6hqlkoRbmm1LArc="] = ' ',
            ["Q3T2C7GGMs2ogZzrxN66invX+7U="] = '!',
            ["QQMGKwEGnyKlaZrqXktlSdvExkk="] = '\'',
            ["f6oQreZYJJH8taqQC3tW370ZFT0="] = ',',
            ["4u+1nZW6+xClbwTaLY0dOJfaTHk="] = '-',
            ["zTRCSfTeL4UVWfxJyIg6nl4zFLA="] = '.',
            ["zY5Ju0enD2Hw/0hxtZexA/Z19SI="] = '0',
            ["wIHj/AR210L+LCu3KpRA9MNw4Kw="] = '3',
            ["d3wU71H/07Ww+mgn3xIsnfHnFVY="] = '4',
            ["CXvZwKcyY7h96M7dmT3vslvwKzs="] = '5',
            ["srHlp7rSS42NCBaSGHa4omqGvrA="] = '?',
            ["7Qa8kA30q08JCIOeJqJcskkijzM="] = 'A',
            ["3+IW2ODznl7WybdtxK+1ZUJMvwE="] = 'B',
            ["BetKtZEQTw9cUdIEPSVHJmUiAGI="] = 'C',
            ["GwkY1qmkQ6rmf7VZ/hxhYW5xWX8="] = 'D',
            ["gbS08yGSswpO3Is496ddSEQfZ/4="] = 'E',
            ["E1Jkk05KNzdQBqdf2d/WoEACKzM="] = 'F',
            ["V2axgNZDhLrE2qqB0dBgx5eJF04="] = 'G',
            ["cknakbZSkOxBPS3RLBWjBZxLTPw="] = 'H',
            ["7PUDJ778JXhHCieaYVVQFZ3cVfI="] = 'I',
            ["jctSOm2sCO7ubBDI3qOIEAOCQnM="] = 'J',
            ["VOr3EZsnVlUwRFTUTrKZKMmyabA="] = 'K',
            ["vcb2zIa8Si9CvMdEl0ATjabAHRo="] = 'L',
            ["B91kv07L7rqYa3unteFS+tHxgk4="] = 'M',
            ["5fE8t9cu2N+Yh4riyhtSYTE4MPI="] = 'N',
            ["HefzvWm2ZAu8lgYsfGY6EuSHbTE="] = 'O',
            ["vKWDfL/8xPLZwl6EG7VWUSGFEGQ="] = 'P',
            ["CdIuq5/Zi2FH6C5Yt533MRFV3+A="] = 'Q',
            ["7i2CG1nvUER/zI6vkjhtmsysRl8="] = 'R',
            ["M0I++ccn6VtfRGNpiWiyeQWxPbc="] = 'S',
            ["fYKLf2SFOjpx26gHjfTnfshOhpI="] = 'T',
            ["vLIN9yT1JVkkBz17FU46ix6FtJ8="] = 'U',
            ["F0a1b6/atpKcWdmNq95ITq0rfFo="] = 'V',
            ["d2iVxl0+kgwcE5G2EHbiemXjI8Y="] = 'W',
            ["SJ8aLVlAmELyWFJ9P5WdjDL1/hI="] = 'Y',
            ["CzDh7vK0vCUJWgHsoSkOBiDb4Lo="] = 'a',
            ["NOFwb7WDnalRw8grTvaFMGY8dDg="] = 'b',
            ["4vDibdsYAtC0VNJB8LB1C1otJAs="] = 'c',
            ["N87Arn2vDVUIiPxNAuVlO929jps="] = 'd',
            ["e//7SgGN6qqE95Cxcyh+ODTHjJ8="] = 'e',
            ["5zJNXGVJkp15b2pnR3y1vLO50Ps="] = 'f',
            ["vwMwUdiZObFRR0DAdYBPVvQZwHs="] = 'g',
            ["JDAW4AisniHQKFKqeUqKAJcD1Rg="] = 'h',
            ["t6WMMw/d4SurcXJBJQplpVGz4SQ="] = 'i',
            ["uuBDW42bS2Qdwh8n2uO7yVER9UM="] = 'j',
            ["8s4cB9lXbT4E5EOx1LG9D8UF0ag="] = 'k',
            ["dcolyDCfeAm2Zsax1hY9YQHpBoA="] = 'l',
            ["HUgiynTwHrM8wT/v3nGgyV1OhBo="] = 'm',
            ["Sb7Acxlayeu5mxEXZ9WAj+tqY5U="] = 'n',
            ["KNTrjhsU6yWSNfI5k8jLSGsUuco="] = 'o',
            ["/1g9elqqym5e6jzUJOg2HDucGQQ="] = 'p',
            ["ksQCe4//Cj8hOfcsnTKjvXHVnyE="] = 'r',
            ["DS8Q4N7Ho2eOOuZsIzxhdSYMxV0="] = 's',
            ["3Ni4mmeW1GwC5ty+rdzlR8g6Q9M="] = 't',
            ["hgU2Pr6dcKuqZJDb7RtPE5bDmhc="] = 'u',
            ["UKB1M6F6bWcRVbKAY/0egVjcsko="] = 'v',
            ["c/VHzQgf2zAeiBjqsMHeSk9vQPI="] = 'w',
            ["TqJ6Nx01P000lvqUyJmMPdTrtAc="] = 'x',
            ["D0QGmchXLhyU8RwOQ1vecU/xY/k="] = 'y',
            ["KZUYg8EgW4VdXND4vLHWqzliCnE="] = '“',
            ["iZNuH162Ncgkqd1ghijWORNhafM="] = '”',
        };

        public static string CutsceneToText(Cutscene cutscene, Dictionary<string, char> translationTable, bool includeInstructionIndex = false, bool normalCutscene = true)
        {
            var str = new StringBuilder();

            var prepend = 5;

            if (includeInstructionIndex)
                prepend += 7;

            var indent = 0;

            bool firstWriteOnLine = true;

            void write(string text, bool lineBreakAfter = false)
            {
                if (firstWriteOnLine && indent > 0)
                    str.Append($"{new string(' ', indent * 2 + prepend)}");

                if (text != null)
                    str.Append(text);

                if (lineBreakAfter)
                {
                    str.AppendLine();
                    firstWriteOnLine = true;
                }
                else
                {
                    firstWriteOnLine = false;
                }
            }
            void writeLine(string text) => write(text, lineBreakAfter: true);

            string[] fontHashes;

            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
                fontHashes = cutscene.Font?.CharactersImgData.Select(x => Convert.ToBase64String(sha1.ComputeHash(x))).ToArray();

            var instructionIndex = 0;

            foreach (var instruction in normalCutscene ? cutscene.Cutscene_Normal.Instructions : cutscene.Cutscene_Skip.Instructions)
            {
                if (includeInstructionIndex)
                    write($"{$"[{instructionIndex}] ",-7}");

                write($"{instruction.FrameIndex,-5}");

                if (Enum.IsDefined(typeof(CutsceneInstruction.InstructionType), instruction.Type))
                    writeLine($"{instruction.Type}");
                else
                    writeLine($"Instruction_{(int)instruction.Type}");

                indent++;

                switch (instruction.Type)
                {
                    case CutsceneInstruction.InstructionType.DrawText:
                        var data_DrawText = (CutsceneInstructionData_DrawText)instruction.Data;
                        writeLine($"Character = {data_DrawText.CharacterName}");
                        writeLine($"Value_02 = {data_DrawText.Byte_01}");

                        indent++;

                        foreach (var cmd in data_DrawText.TextCommands)
                        {
                            switch (cmd.Type)
                            {
                                case CutsceneInstructionData_DrawText.TextCommand.CommandType.DrawChar:
                                    var hash = fontHashes[cmd.Command];

                                    if (translationTable.ContainsKey(hash))
                                        write(translationTable[hash].ToString());
                                    else
                                        write($"❔");
                                    break;

                                case CutsceneInstructionData_DrawText.TextCommand.CommandType.CMD_0:
                                case CutsceneInstructionData_DrawText.TextCommand.CommandType.Blank:
                                case CutsceneInstructionData_DrawText.TextCommand.CommandType.Delay:
                                case CutsceneInstructionData_DrawText.TextCommand.CommandType.PlaySound:
                                    write($"[{cmd.Type}:{cmd.CommandParam}]");
                                    break;

                                case CutsceneInstructionData_DrawText.TextCommand.CommandType.LineBreak:
                                case CutsceneInstructionData_DrawText.TextCommand.CommandType.End:
                                case CutsceneInstructionData_DrawText.TextCommand.CommandType.WaitForInput:
                                case CutsceneInstructionData_DrawText.TextCommand.CommandType.Clear:
                                    write($"[{cmd.Type}]");
                                    break;

                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }

                        writeLine(null);

                        indent--;

                        break;

                    case CutsceneInstruction.InstructionType.Instruction_2:
                        var data_2 = (CutsceneInstructionData_2)instruction.Data;
                        writeLine($"Object = {data_2.ObjIndex}");
                        writeLine($"Param_00 = {data_2.Short_Param00}");
                        writeLine($"Param_02 = {data_2.Short_Param02}");
                        writeLine($"Param_04 = {data_2.Short_Param04}");
                        break;

                    case CutsceneInstruction.InstructionType.Instruction_3:
                        var data_3 = (CutsceneInstructionData_3)instruction.Data;
                        writeLine($"Flags = {data_3.Flags}");
                        writeLine($"Value_02 = {data_3.Int_02}");
                        break;

                    case CutsceneInstruction.InstructionType.CreateObj3D:
                        var data_CreateObj3D = (CutsceneInstructionData_CreateObj3D)instruction.Data;
                        writeLine($"SecondaryType = {data_CreateObj3D.SecondaryType}");
                        writeLine($"Value_02 = {data_CreateObj3D.Int_02}");
                        break;

                    case CutsceneInstruction.InstructionType.Instruction_5:
                        var data_5 = (CutsceneInstructionData_5)instruction.Data;
                        writeLine($"Value_00 = {data_5.Byte_00}");
                        break;

                    case CutsceneInstruction.InstructionType.SetObjPosFromPath:
                        var data_6 = (CutsceneInstructionData_SetObjPosFromPath)instruction.Data;
                        writeLine($"Object = {data_6.ObjIndex}");
                        writeLine($"MovementPathIndex = {data_6.MovementPathIndex}");
                        writeLine($"MovementPathDistance = {data_6.MovementPathDistance}");
                        writeLine($"Param_04 = {data_6.Short_Param04}");

                        if (data_6.FlipX != null)
                            writeLine($"FlipX = {data_6.FlipX}");
                        break;

                    case CutsceneInstruction.InstructionType.SetObj2DAnimation:
                        var data_SetObjAnimation = (CutsceneInstructionData_SetObjAnimation)instruction.Data;
                        writeLine($"Object = {data_SetObjAnimation.ObjIndex}");
                        writeLine($"Animation = {data_SetObjAnimation.AnimIndex}");
                        break;

                    case CutsceneInstruction.InstructionType.CreateObj2D:
                        var data_CreateObj = (CutsceneInstructionData_CreateObj)instruction.Data;
                        writeLine($"Object = {data_CreateObj.ObjIndex}");
                        writeLine($"Value_02 = {data_CreateObj.Int_02}");
                        break;

                    case CutsceneInstruction.InstructionType.Instruction_9:
                        var data_9 = (CutsceneInstructionData_9)instruction.Data;
                        writeLine($"Object = {data_9.ObjIndex}");
                        break;

                    case CutsceneInstruction.InstructionType.CreateTextBox:
                        // Nothing to log
                        break;

                    case CutsceneInstruction.InstructionType.ChangeSector:
                        var data_ChangeSector = (CutsceneInstructionData_ChangeSector)instruction.Data;
                        writeLine($"Sector = {data_ChangeSector.SectorIndex}");
                        break;

                    case CutsceneInstruction.InstructionType.SetObjPos:
                        var data_SetObjPos = (CutsceneInstructionData_SetObjPos)instruction.Data;
                        writeLine($"Object = {data_SetObjPos.ObjIndex}");
                        writeLine($"RelativeObject = {data_SetObjPos.PositionRelativeObjIndex}");
                        writeLine($"Position = ({data_SetObjPos.Position.X}, {data_SetObjPos.Position.Y}, {data_SetObjPos.Position.Z})");
                        break;

                    case CutsceneInstruction.InstructionType.ClearVRAM:
                        var data_ClearVRAM = (CutsceneInstructionData_ClearVRAM)instruction.Data;
                        writeLine($"Value_00 = {data_ClearVRAM.Byte_00}");
                        writeLine($"Value_02 = {data_ClearVRAM.Int_02}");
                        break;

                    case CutsceneInstruction.InstructionType.SetCutsceneState:
                        var data_SetCutsceneState = (CutsceneInstructionData_SetCutsceneState)instruction.Data;
                        writeLine($"State = {data_SetCutsceneState.State}");
                        break;

                    case CutsceneInstruction.InstructionType.Terminator:
                    case CutsceneInstruction.InstructionType.Special:
                        // Do nothing
                        break;

                    default:
                        var data_Default = (CutsceneInstructionData_Default)instruction.Data;
                        writeLine($"Data = {data_Default.RawBytes.ToHexString()}");
                        break;
                }

                indent--;

                instructionIndex++;
            }

            return str.ToString();
        }
    }
}