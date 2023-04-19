using System;

namespace BinarySerializer.Klonoa.LV
{
    public class PuppetCommand : BinarySerializable, ISerializerShortLog
    {
        public CommandType Type { get; set; }

        #region Data
        public PuppetCommandData32A Data32A { get; set; }
        public PuppetCommandData32B Data32B { get; set; }
        public PuppetCommandData64A Data64A { get; set; }
        public PuppetCommandData64B Data64B { get; set; }
        public PuppetCommandData64C Data64C { get; set; }
        public PuppetCommandData64D Data64D { get; set; }
        public PuppetCommandData64E Data64E { get; set; }
        public PuppetCommandData64F Data64F { get; set; }
        public PuppetCommandData96A Data96A { get; set; }
        public PuppetCommandData96B Data96B { get; set; }
        public PuppetCommandData96C Data96C { get; set; }
        public PuppetCommandData96D Data96D { get; set; }
        public PuppetCommandData96E Data96E { get; set; }
        public PuppetCommandData96F Data96F { get; set; }
        public PuppetCommandData128A Data128A { get; set; }
        public PuppetCommandData128B Data128B { get; set; }
        public PuppetCommandData128C Data128C { get; set; }
        public PuppetCommandData160A Data160A { get; set; }
        #endregion

        public override void SerializeImpl(SerializerObject s)
        {
            Type = s.Serialize<CommandType>(Type, nameof(Type));
            if (!Enum.IsDefined(typeof(CommandType), Type))
            {
                Type = CommandType.Invalid;
                return;
            }
                
            switch (Type)
            {
                #region Control
                case CommandType.Control_Call:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Control_End:
                    s.SerializePadding(2);
                    break;
                case CommandType.Control_Time:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Control_Subroutine:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Control_Return:
                    s.SerializePadding(2);
                    break;
                case CommandType.Control_Type:
                    Data64A = s.SerializeObject<PuppetCommandData64A>(Data64A, name: nameof(Data64A));
                    break;
                case CommandType.Control_Position:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Control_Rotation:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Control_Draw:
                    Data32B = s.SerializeObject<PuppetCommandData32B>(Data32B, name: nameof(Data32B));
                    break;
                case CommandType.Control_Jump:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Control_Flag:
                    Data96A = s.SerializeObject<PuppetCommandData96A>(Data96A, name: nameof(Data96A));
                    break;
                case CommandType.Control_FlagJ:
                    Data96A = s.SerializeObject<PuppetCommandData96A>(Data96A, name: nameof(Data96A));
                    break;
                case CommandType.Control_PositionSpeed:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Control_PositionAcceleration:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Control_RotationSpeed:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Control_RotationAcceleration:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Control_PositionMV:
                    Data128B = s.SerializeObject<PuppetCommandData128B>(Data128B, name: nameof(Data128B));
                    break;
                case CommandType.Control_CallClear:
                    s.SerializePadding(2);
                    break;
                case CommandType.Control_RotationMV:
                    Data128B = s.SerializeObject<PuppetCommandData128B>(Data128B, name: nameof(Data128B));
                    break;
                case CommandType.Control_TimeMP:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Control_TimeMF:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Control_NLight:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Control_LightColor:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Control_AmbientColor:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Control_Size:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Control_MessageData:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Control_MessageStart:
                    s.SerializePadding(2);
                    break;
                case CommandType.Control_MessageWindow:
                    Data32B = s.SerializeObject<PuppetCommandData32B>(Data32B, name: nameof(Data32B));
                    break;
                case CommandType.Control_MessageSpeed:
                    s.SerializePadding(2);
                    break;
                case CommandType.Control_MessageReta:
                    s.SerializePadding(2);
                    break;
                case CommandType.Control_GetFT:
                    Data64A = s.SerializeObject<PuppetCommandData64A>(Data64A, name: nameof(Data64A));
                    break;
                case CommandType.Control_PositionMVP:
                    Data128B = s.SerializeObject<PuppetCommandData128B>(Data128B, name: nameof(Data128B));
                    break;
                case CommandType.Control_RotationMVP:
                    Data128B = s.SerializeObject<PuppetCommandData128B>(Data128B, name: nameof(Data128B));
                    break;
                case CommandType.Control_Scale:
                    Data64C = s.SerializeObject<PuppetCommandData64C>(Data64C, name: nameof(Data64C));
                    break;
                case CommandType.Control_PositionSpeed1:
                    Data64C = s.SerializeObject<PuppetCommandData64C>(Data64C, name: nameof(Data64C));
                    break;
                case CommandType.Control_RotationSpeed1:
                    Data64C = s.SerializeObject<PuppetCommandData64C>(Data64C, name: nameof(Data64C));
                    break;
                case CommandType.Control_LightNumber:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Control_LightSW:
                    Data64A = s.SerializeObject<PuppetCommandData64A>(Data64A, name: nameof(Data64A));
                    break;
                case CommandType.Control_LightColorSpeed:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Control_LightColorAcceleration:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Control_LightColorMV:
                    Data128B = s.SerializeObject<PuppetCommandData128B>(Data128B, name: nameof(Data128B));
                    break;
                case CommandType.Control_LightColorMVP:
                    Data128B = s.SerializeObject<PuppetCommandData128B>(Data128B, name: nameof(Data128B));
                    break;
                case CommandType.Control_LightColorSpeed1:
                    Data64C = s.SerializeObject<PuppetCommandData64C>(Data64C, name: nameof(Data64C));
                    break;
                case CommandType.Control_LightRotationSpeed:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Control_LightRotationAcceleration:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Control_LightRotationMV:
                    Data128B = s.SerializeObject<PuppetCommandData128B>(Data128B, name: nameof(Data128B));
                    break;
                case CommandType.Control_LightRotationMVP:
                    Data128B = s.SerializeObject<PuppetCommandData128B>(Data128B, name: nameof(Data128B));
                    break;
                case CommandType.Control_LightRotationSpeed1:
                    Data64C = s.SerializeObject<PuppetCommandData64C>(Data64C, name: nameof(Data64C));
                    break;
                case CommandType.Control_Effect:
                    Data64A = s.SerializeObject<PuppetCommandData64A>(Data64A, name: nameof(Data64A));
                    break;
                case CommandType.Control_Col:
                    Data64E = s.SerializeObject<PuppetCommandData64E>(Data64E, name: nameof(Data64E));
                    break;
                case CommandType.Control_ColIn:
                    Data64E = s.SerializeObject<PuppetCommandData64E>(Data64E, name: nameof(Data64E));
                    break;
                case CommandType.Control_ColOut:
                    Data64E = s.SerializeObject<PuppetCommandData64E>(Data64E, name: nameof(Data64E));
                    break;
                case CommandType.Control_PositionCP:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Control_RotationCP:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Control_PositionMVPC:
                    Data160A = s.SerializeObject<PuppetCommandData160A>(Data160A, name: nameof(Data160A));
                    break;
                case CommandType.Control_LightRotation:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Control_VtMode:
                    Data32B = s.SerializeObject<PuppetCommandData32B>(Data32B, name: nameof(Data32B));
                    break;
                case CommandType.Control_PositionCPT:
                    Data160A = s.SerializeObject<PuppetCommandData160A>(Data160A, name: nameof(Data160A));
                    break;
                case CommandType.Control_Id:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Control_Chr:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Control_Exit:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Control_ACol:
                    Data64E = s.SerializeObject<PuppetCommandData64E>(Data64E, name: nameof(Data64E));
                    break;
                case CommandType.Control_AColIn:
                    Data64E = s.SerializeObject<PuppetCommandData64E>(Data64E, name: nameof(Data64E));
                    break;
                case CommandType.Control_AColOut:
                    Data64E = s.SerializeObject<PuppetCommandData64E>(Data64E, name: nameof(Data64E));
                    break;
                case CommandType.Control_Econt:
                    Data64A = s.SerializeObject<PuppetCommandData64A>(Data64A, name: nameof(Data64A));
                    break;
                case CommandType.Control_Crossfade:
                    Data32B = s.SerializeObject<PuppetCommandData32B>(Data32B, name: nameof(Data32B));
                    break;
                case CommandType.Control_CrossfadeW:
                    s.SerializePadding(2);
                    break;
                case CommandType.Control_PositionMH:
                    Data160A = s.SerializeObject<PuppetCommandData160A>(Data160A, name: nameof(Data160A));
                    break;
                case CommandType.Control_RotationMH:
                    Data160A = s.SerializeObject<PuppetCommandData160A>(Data160A, name: nameof(Data160A));
                    break;
                case CommandType.Control_PositionMHP:
                    Data160A = s.SerializeObject<PuppetCommandData160A>(Data160A, name: nameof(Data160A));
                    break;
                case CommandType.Control_RotationMHP:
                    Data160A = s.SerializeObject<PuppetCommandData160A>(Data160A, name: nameof(Data160A));
                    break;
                case CommandType.Control_MkMat:
                    Data32B = s.SerializeObject<PuppetCommandData32B>(Data32B, name: nameof(Data32B));
                    break;
                case CommandType.Control_PositionCPM:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Control_CpyFT:
                    Data32B = s.SerializeObject<PuppetCommandData32B>(Data32B, name: nameof(Data32B));
                    break;
                case CommandType.Control_TimeFT:
                    s.SerializePadding(2);
                    break;
                case CommandType.Control_Loop:
                    Data64A = s.SerializeObject<PuppetCommandData64A>(Data64A, name: nameof(Data64A));
                    break;
                case CommandType.Control_BgMode:
                    Data64A = s.SerializeObject<PuppetCommandData64A>(Data64A, name: nameof(Data64A));
                    break;
                case CommandType.Control_FogNear:
                    Data64B = s.SerializeObject<PuppetCommandData64B>(Data64B, name: nameof(Data64B));
                    break;
                case CommandType.Control_FogFar:
                    Data64B = s.SerializeObject<PuppetCommandData64B>(Data64B, name: nameof(Data64B));
                    break;
                case CommandType.Control_PtNext:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Control_VtWave:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Control_PtDel:
                    s.SerializePadding(2);
                    break;
                case CommandType.Control_MessageNSkip:
                    Data32B = s.SerializeObject<PuppetCommandData32B>(Data32B, name: nameof(Data32B));
                    break;
                case CommandType.Control_NTSCJ:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Control_PALJ:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                #endregion

                #region Motion
                case CommandType.Motion_Play:
                    Data64A = s.SerializeObject<PuppetCommandData64A>(Data64A, name: nameof(Data64A));
                    break;
                case CommandType.Motion_Wait:
                    Data32B = s.SerializeObject<PuppetCommandData32B>(Data32B, name: nameof(Data32B));
                    break;
                case CommandType.Motion_MimeSet:
                    Data32B = s.SerializeObject<PuppetCommandData32B>(Data32B, name: nameof(Data32B));
                    break;
                case CommandType.Motion_Lips:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Motion_PlayS:
                    Data64A = s.SerializeObject<PuppetCommandData64A>(Data64A, name: nameof(Data64A));
                    break;
                case CommandType.Motion_Clip:
                    Data32B = s.SerializeObject<PuppetCommandData32B>(Data32B, name: nameof(Data32B));
                    break;
                case CommandType.Motion_Line:
                    Data32B = s.SerializeObject<PuppetCommandData32B>(Data32B, name: nameof(Data32B));
                    break;
                case CommandType.Motion_LipsData:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Motion_PlayE:
                    Data64A = s.SerializeObject<PuppetCommandData64A>(Data64A, name: nameof(Data64A));
                    break;
                case CommandType.Motion_PlaySE:
                    Data64A = s.SerializeObject<PuppetCommandData64A>(Data64A, name: nameof(Data64A));
                    break;
                #endregion

                #region Camera
                case CommandType.Camera_Position:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Camera_Work:
                    Data32B = s.SerializeObject<PuppetCommandData32B>(Data32B, name: nameof(Data32B));
                    break;
                case CommandType.Camera_Hold:
                    Data32B = s.SerializeObject<PuppetCommandData32B>(Data32B, name: nameof(Data32B));
                    break;
                case CommandType.Camera_Vector:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Camera_Mat:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Camera_InteHold:
                    Data32B = s.SerializeObject<PuppetCommandData32B>(Data32B, name: nameof(Data32B));
                    break;
                case CommandType.Camera_InteVector:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Camera_InteMat:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Camera_Speed:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Camera_Acceleration:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Camera_IntePosition:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Camera_InteSpeed:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Camera_InteAcceleration:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Camera_AngleSet:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Camera_AngleSpeed:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Camera_AngleAcceleration:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Camera_LenSet:
                    Data64C = s.SerializeObject<PuppetCommandData64C>(Data64C, name: nameof(Data64C));
                    break;
                case CommandType.Camera_LenSpeed:
                    Data64C = s.SerializeObject<PuppetCommandData64C>(Data64C, name: nameof(Data64C));
                    break;
                case CommandType.Camera_LenAcceleration:
                    Data64C = s.SerializeObject<PuppetCommandData64C>(Data64C, name: nameof(Data64C));
                    break;
                case CommandType.Camera_MatG:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Camera_InteMatG:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Camera_MVP:
                    Data128B = s.SerializeObject<PuppetCommandData128B>(Data128B, name: nameof(Data128B));
                    break;
                case CommandType.Camera_InteMVP:
                    Data128B = s.SerializeObject<PuppetCommandData128B>(Data128B, name: nameof(Data128B));
                    break;
                case CommandType.Camera_VecG:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Camera_InteVecG:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Camera_LrSet:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Camera_LrSpeed:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Camera_LrAcceleration:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Camera_LrMVP:
                    Data128B = s.SerializeObject<PuppetCommandData128B>(Data128B, name: nameof(Data128B));
                    break;
                case CommandType.Camera_InteLrSet:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Camera_InteLrSpeed:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Camera_InteLrAcceleration:
                    Data128A = s.SerializeObject<PuppetCommandData128A>(Data128A, name: nameof(Data128A));
                    break;
                case CommandType.Camera_InteLrMVP:
                    Data128B = s.SerializeObject<PuppetCommandData128B>(Data128B, name: nameof(Data128B));
                    break;
                case CommandType.Camera_LrMV:
                    Data128B = s.SerializeObject<PuppetCommandData128B>(Data128B, name: nameof(Data128B));
                    break;
                case CommandType.Camera_InteLrMV:
                    Data128B = s.SerializeObject<PuppetCommandData128B>(Data128B, name: nameof(Data128B));
                    break;
                case CommandType.Camera_MV:
                    Data128B = s.SerializeObject<PuppetCommandData128B>(Data128B, name: nameof(Data128B));
                    break;
                case CommandType.Camera_InteMV:
                    Data128B = s.SerializeObject<PuppetCommandData128B>(Data128B, name: nameof(Data128B));
                    break;
                case CommandType.Camera_AngleMV:
                    Data128B = s.SerializeObject<PuppetCommandData128B>(Data128B, name: nameof(Data128B));
                    break;
                case CommandType.Camera_AngleMVP:
                    Data128B = s.SerializeObject<PuppetCommandData128B>(Data128B, name: nameof(Data128B));
                    break;
                case CommandType.Camera_Speed1:
                    Data64C = s.SerializeObject<PuppetCommandData64C>(Data64C, name: nameof(Data64C));
                    break;
                case CommandType.Camera_InteSpeed1:
                    Data64C = s.SerializeObject<PuppetCommandData64C>(Data64C, name: nameof(Data64C));
                    break;
                case CommandType.Camera_LrSpeed1:
                    Data64C = s.SerializeObject<PuppetCommandData64C>(Data64C, name: nameof(Data64C));
                    break;
                case CommandType.Camera_InteLrSpeed1:
                    Data64C = s.SerializeObject<PuppetCommandData64C>(Data64C, name: nameof(Data64C));
                    break;
                case CommandType.Camera_AngleSpeed1:
                    Data64C = s.SerializeObject<PuppetCommandData64C>(Data64C, name: nameof(Data64C));
                    break;
                case CommandType.Camera_Data:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Camera_LenMVP:
                    Data64F = s.SerializeObject<PuppetCommandData64F>(Data64F, name: nameof(Data64F));
                    break;
                case CommandType.Camera_LenMV:
                    Data64F = s.SerializeObject<PuppetCommandData64F>(Data64F, name: nameof(Data64F));
                    break;
                case CommandType.Camera_ProjSet:
                    Data64C = s.SerializeObject<PuppetCommandData64C>(Data64C, name: nameof(Data64C));
                    break;
                case CommandType.Camera_ProjSpeed:
                    Data64C = s.SerializeObject<PuppetCommandData64C>(Data64C, name: nameof(Data64C));
                    break;
                case CommandType.Camera_ProjAcceleration:
                    Data64C = s.SerializeObject<PuppetCommandData64C>(Data64C, name: nameof(Data64C));
                    break;
                case CommandType.Camera_ProjMVP:
                    Data64F = s.SerializeObject<PuppetCommandData64F>(Data64F, name: nameof(Data64F));
                    break;
                case CommandType.Camera_ProjMV:
                    Data64F = s.SerializeObject<PuppetCommandData64F>(Data64F, name: nameof(Data64F));
                    break;
                case CommandType.Camera_MHP:
                    Data160A = s.SerializeObject<PuppetCommandData160A>(Data160A, name: nameof(Data160A));
                    break;
                case CommandType.Camera_InteMHP:
                    Data160A = s.SerializeObject<PuppetCommandData160A>(Data160A, name: nameof(Data160A));
                    break;
                case CommandType.Camera_LrMHP:
                    Data160A = s.SerializeObject<PuppetCommandData160A>(Data160A, name: nameof(Data160A));
                    break;
                case CommandType.Camera_InteLrMHP:
                    Data160A = s.SerializeObject<PuppetCommandData160A>(Data160A, name: nameof(Data160A));
                    break;
                case CommandType.Camera_LrMH:
                    Data160A = s.SerializeObject<PuppetCommandData160A>(Data160A, name: nameof(Data160A));
                    break;
                case CommandType.Camera_InteLrMH:
                    Data160A = s.SerializeObject<PuppetCommandData160A>(Data160A, name: nameof(Data160A));
                    break;
                case CommandType.Camera_MH:
                    Data160A = s.SerializeObject<PuppetCommandData160A>(Data160A, name: nameof(Data160A));
                    break;
                case CommandType.Camera_InteMH:
                    Data160A = s.SerializeObject<PuppetCommandData160A>(Data160A, name: nameof(Data160A));
                    break;
                case CommandType.Camera_AngleMH:
                    Data160A = s.SerializeObject<PuppetCommandData160A>(Data160A, name: nameof(Data160A));
                    break;
                case CommandType.Camera_AngleMHP:
                    Data160A = s.SerializeObject<PuppetCommandData160A>(Data160A, name: nameof(Data160A));
                    break;
                #endregion

                #region Game
                case CommandType.Game_RtData:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Game_RtPosition:
                    Data64B = s.SerializeObject<PuppetCommandData64B>(Data64B, name: nameof(Data64B));
                    break;
                case CommandType.Game_RtSpeed:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Game_RtAcceleration:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Game_RtOff:
                    s.SerializePadding(2);
                    break;
                case CommandType.Game_RtMV:
                    Data64D = s.SerializeObject<PuppetCommandData64D>(Data64D, name: nameof(Data64D));
                    break;
                case CommandType.Game_RtMVP:
                    Data64D = s.SerializeObject<PuppetCommandData64D>(Data64D, name: nameof(Data64D));
                    break;
                case CommandType.Game_StRt:
                    Data96A = s.SerializeObject<PuppetCommandData96A>(Data96A, name: nameof(Data96A));
                    break;
                case CommandType.Game_StSW:
                    Data64A = s.SerializeObject<PuppetCommandData64A>(Data64A, name: nameof(Data64A));
                    break;
                case CommandType.Game_RtWait:
                    Data96A = s.SerializeObject<PuppetCommandData96A>(Data96A, name: nameof(Data96A));
                    break;
                case CommandType.Game_Copy:
                    s.SerializePadding(2);
                    break;
                case CommandType.Game_CamCat:
                    s.SerializePadding(2);
                    break;
                case CommandType.Game_CamRel:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Game_W2Rt:
                    Data64B = s.SerializeObject<PuppetCommandData64B>(Data64B, name: nameof(Data64B));
                    break;
                case CommandType.Game_Rt2W:
                    Data32B = s.SerializeObject<PuppetCommandData32B>(Data32B, name: nameof(Data32B));
                    break;
                case CommandType.Game_GameJ:
                    Data64A = s.SerializeObject<PuppetCommandData64A>(Data64A, name: nameof(Data64A));
                    break;
                case CommandType.Game_GameR:
                    Data32B = s.SerializeObject<PuppetCommandData32B>(Data32B, name: nameof(Data32B));
                    break;
                case CommandType.Game_Back:
                    Data64B = s.SerializeObject<PuppetCommandData64B>(Data64B, name: nameof(Data64B));
                    break;
                case CommandType.Game_PosJ:
                    Data96B = s.SerializeObject<PuppetCommandData96B>(Data96B, name: nameof(Data96B));
                    break;
                case CommandType.Game_Key:
                    Data64B = s.SerializeObject<PuppetCommandData64B>(Data64B, name: nameof(Data64B));
                    break;
                case CommandType.Game_RtJump:
                    Data128C = s.SerializeObject<PuppetCommandData128C>(Data128C, name: nameof(Data128C));
                    break;
                case CommandType.Game_Cent:
                    Data32B = s.SerializeObject<PuppetCommandData32B>(Data32B, name: nameof(Data32B));
                    break;
                case CommandType.Game_KeyJ:
                    Data96A = s.SerializeObject<PuppetCommandData96A>(Data96A, name: nameof(Data96A));
                    break;
                case CommandType.Game_Jump:
                    Data96D = s.SerializeObject<PuppetCommandData96D>(Data96D, name: nameof(Data96D));
                    break;
                case CommandType.Game_Shadow:
                    Data64C = s.SerializeObject<PuppetCommandData64C>(Data64C, name: nameof(Data64C));
                    break;
                case CommandType.Game_Mir:
                    Data32B = s.SerializeObject<PuppetCommandData32B>(Data32B, name: nameof(Data32B));
                    break;
                case CommandType.Game_MesP:
                    Data64A = s.SerializeObject<PuppetCommandData64A>(Data64A, name: nameof(Data64A));
                    break;
                case CommandType.Game_BackV:
                    Data32B = s.SerializeObject<PuppetCommandData32B>(Data32B, name: nameof(Data32B));
                    break;
                case CommandType.Game_BackY:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Game_Switch:
                    Data64A = s.SerializeObject<PuppetCommandData64A>(Data64A, name: nameof(Data64A));
                    break;
                case CommandType.Game_VisionClear:
                    s.SerializePadding(2);
                    break;
                case CommandType.Game_Reset:
                    s.SerializePadding(2);
                    break;
                case CommandType.Game_NSkip:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Game_LCol:
                    Data64E = s.SerializeObject<PuppetCommandData64E>(Data64E, name: nameof(Data64E));
                    break;
                #endregion
                
                #region Sound
                case CommandType.Sound_Voice:
                    Data32B = s.SerializeObject<PuppetCommandData32B>(Data32B, name: nameof(Data32B));
                    break;
                case CommandType.Sound_SE:
                    Data64B = s.SerializeObject<PuppetCommandData64B>(Data64B, name: nameof(Data64B));
                    break;
                case CommandType.Sound_VoiceData:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Sound_Selp:
                    Data64B = s.SerializeObject<PuppetCommandData64B>(Data64B, name: nameof(Data64B));
                    break;
                case CommandType.Sound_SelpOff:
                    Data64B = s.SerializeObject<PuppetCommandData64B>(Data64B, name: nameof(Data64B));
                    break;
                case CommandType.Sound_ABGM:
                    Data32B = s.SerializeObject<PuppetCommandData32B>(Data32B, name: nameof(Data32B));
                    break;
                case CommandType.Sound_NBGM:
                    Data32B = s.SerializeObject<PuppetCommandData32B>(Data32B, name: nameof(Data32B));
                    break;
                case CommandType.Sound_SPitSet:
                    Data96E = s.SerializeObject<PuppetCommandData96E>(Data96E, name: nameof(Data96E));
                    break;
                case CommandType.Sound_SPitMV:  
                    Data96E = s.SerializeObject<PuppetCommandData96E>(Data96E, name: nameof(Data96E));
                    break;
                case CommandType.Sound_SPitMVP:
                    Data96E = s.SerializeObject<PuppetCommandData96E>(Data96E, name: nameof(Data96E));
                    break;
                case CommandType.Sound_SVolSet:
                    Data96F = s.SerializeObject<PuppetCommandData96F>(Data96F, name: nameof(Data96F));
                    break;
                case CommandType.Sound_SVolMV:
                    Data96F = s.SerializeObject<PuppetCommandData96F>(Data96F, name: nameof(Data96F));
                    break;
                case CommandType.Sound_SVolMVP:
                    Data96F = s.SerializeObject<PuppetCommandData96F>(Data96F, name: nameof(Data96F));
                    break;
                case CommandType.Sound_SEv:
                    Data96D = s.SerializeObject<PuppetCommandData96D>(Data96D, name: nameof(Data96D));
                    break;
                case CommandType.Sound_BGMSync:
                    s.SerializePadding(4);
                    break;
                case CommandType.Sound_AC3J:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                case CommandType.Sound_AC3S:
                    Data32A = s.SerializeObject<PuppetCommandData32A>(Data32A, name: nameof(Data32A));
                    break;
                #endregion

                case CommandType.Invalid:
                    s.SerializePadding(2);
                    break;

                default:
                    throw new BinarySerializableException(this, $"Unimplemented command {Type} (0x{Type.ToString("X")})");
            }
        }

		public string ShortLog => ToString();
		public override string ToString()
        {
            string args = "";
            if (Data32A != null)
                args = $"{Data32A.Short0}";
            else if (Data32B != null)
                args = $"{Data32B.UShort0}";
            else if (Data64A != null)
                args = $"{Data64A.Short0}, {Data64A.Short1}, {Data64A.Short2}";
            else if (Data64B != null)
                args = $"{Data64B.UShort0}, {Data64B.Int0}";
            else if (Data64C != null)
                args = $"{Data64C.UShort0}, {Data64C.Float0}";
            else if (Data64D != null)
                args = $"{Data64D.Byte0}, {Data64D.Byte1}, {Data64D.Int0}";
            else if (Data64E != null)
                args = $"{Data64E.UShort0}, {Data64E.Byte0}, {Data64E.Byte1}, {Data64E.Byte2}, {Data64E.Byte3}";
            else if (Data64F != null)
                args = $"{Data64F.Byte0}, {Data64F.Byte1}, {Data64F.Float0}";
            else if (Data96A != null)
                args = $"{Data96A.Draw}, {Data96A.UShort0}, {Data96A.Short0}, {Data96A.UInt0}";
            else if (Data96B != null)
                args = $"{Data96B.UShort0}, {Data96B.UShort1}, {Data96B.Short0}, {Data96B.Float0}";
            else if (Data96C != null)
                args = $"{Data96C.UShort0}, {Data96C.UInt0}, {Data96C.UInt1}";
            else if (Data96D != null)
                args = $"{Data96D.UShort0}, {Data96D.Int0}, {Data96D.Float0}";
            else if (Data96E != null)
                args = $"{Data96E.Byte0}, {Data96E.Byte1}, {Data96E.Int0}, {Data96E.UInt0}";
            else if (Data96F != null)
                args = $"{Data96F.Byte0}, {Data96F.Byte1}, {Data96F.Float0}, {Data96F.UInt0}";
            else if (Data128A != null)
                args = $"{Data128A.Short0}, {Data128A.Float0}, {Data128A.Float1}, {Data128A.Float2}";
            else if (Data128B != null)
                args = $"{Data128B.Byte0}, {Data128B.Byte1}, {Data128B.Float0}, {Data128B.Float1}, {Data128B.Float2}";
            else if (Data128C != null)
                args = $"{Data128C.UShort0}, {Data128C.UShort1}, {Data128C.Short0}, {Data128C.UInt0}, {Data128C.Short1}, {Data128C.Short2}";
            else if (Data160A != null)
                args = $"{Data160A.Byte0}, {Data160A.Byte1}, {Data160A.Float0}, {Data160A.Float1}, {Data160A.Float2}, {Data160A.Short0}, {Data160A.Short1}";
            return $"{Type}({args})";
        }

        // TODO: Find out what these terms are supposed to mean:
        //       - Inte(rpolate?)
        //       - Len
        //       - Lr
        //       - Proj(ection?)
        //       - Rt (most likely route)
        //       - FT
        //       - Spit
        //       - M(at?)/V(ec?)/P/C/H
        public enum CommandType : short
        {
            #region Control
            Control_Call = 0x0000,
            // Control_Inc = 0x0000,
            // Control_Def = 0x0001,
            Control_End = 0x0001,
            // Control_Data = 0x0002,
            Control_Time = 0x0002,
            // Control_Def2 = 0x0003,
            Control_Subroutine = 0x0003,
            // Control_Data4 = 0x0004,
            Control_Return = 0x0004,
            // Control_DtSize = 0x0005,
            Control_Type = 0x0005,
            // Control_LipT = 0x0006,
            Control_Position = 0x0006,
            // Control_PrdsNum = 0x0007,
            Control_Rotation = 0x0007,
            Control_Draw = 0x0008,
            Control_Jump = 0x0009,
            Control_Flag = 0x000A,
            Control_FlagJ = 0x000B,
            Control_PositionSpeed = 0x000C,
            Control_PositionAcceleration = 0x000D,
            Control_RotationSpeed = 0x000E,
            Control_RotationAcceleration = 0x000F,
            Control_PositionMV = 0x0010,
            Control_CallClear = 0x0011,
            Control_RotationMV = 0x0012,
            Control_TimeMP = 0x0013,
            Control_TimeMF = 0x0014,
            Control_NLight = 0x0015,
            Control_LightColor = 0x0016,
            Control_AmbientColor = 0x0017,
            Control_Size = 0x0018,
            Control_MessageData = 0x0019,
            Control_MessageStart = 0x001A,
            Control_MessageWindow = 0x001B,
            Control_MessageSpeed = 0x001C,
            Control_MessageReta = 0x001D,
            Control_GetFT = 0x001E,
            Control_PositionMVP = 0x001F,
            Control_RotationMVP = 0x0020,
            Control_Scale = 0x0021,
            Control_PositionSpeed1 = 0x0022,
            Control_RotationSpeed1 = 0x0023,
            Control_LightNumber = 0x0024,
            Control_LightSW = 0x0025,
            Control_LightColorSpeed = 0x0026,
            Control_LightColorAcceleration = 0x0027,
            Control_LightColorMV = 0x0028,
            Control_LightColorMVP = 0x0029,
            Control_LightColorSpeed1 = 0x002A,
            Control_LightRotationSpeed = 0x002B,
            Control_LightRotationAcceleration = 0x002C,
            Control_LightRotationMV = 0x002D,
            Control_LightRotationMVP = 0x002E,
            Control_LightRotationSpeed1 = 0x002F,
            Control_Effect = 0x0030,
            Control_Col = 0x0031,
            Control_ColIn = 0x0032,
            Control_ColOut = 0x0033,
            Control_PositionCP = 0x0034,
            Control_RotationCP = 0x0035,
            Control_PositionMVPC = 0x0036,
            Control_LightRotation = 0x0037,
            Control_VtMode = 0x0038,
            Control_PositionCPT = 0x0039,
            Control_Id = 0x003A,
            Control_Chr = 0x003B,
            Control_Exit = 0x003C,
            Control_ACol = 0x003D,
            Control_AColIn = 0x003E,
            Control_AColOut = 0x003F,
            Control_Econt = 0x0040,
            Control_Crossfade = 0x0041,
            Control_CrossfadeW = 0x0042,
            Control_PositionMH = 0x0043,
            Control_RotationMH = 0x0044,
            Control_PositionMHP = 0x0045,
            Control_RotationMHP = 0x0046,
            Control_MkMat = 0x0047,
            Control_PositionCPM = 0x0048,
            Control_CpyFT = 0x0049,
            Control_TimeFT = 0x004A,
            Control_Loop = 0x004B,
            Control_BgMode = 0x004C,
            Control_FogNear = 0x004D,
            Control_FogFar = 0x004E,
            Control_PtNext = 0x004F,
            Control_VtWave = 0x0050,
            Control_PtDel = 0x0051,
            Control_MessageNSkip = 0x0052,
            Control_NTSCJ = 0x0053,
            Control_PALJ = 0x0054,
            Control_PNum = 0x0055,
            #endregion

            #region Motion
            Motion_Play = 0x0100,
            Motion_Wait = 0x0101,
            Motion_MimeSet = 0x0102,
            Motion_Lips = 0x0103,
            Motion_PlayS = 0x0104,
            Motion_Clip = 0x0105,
            Motion_Line = 0x0106,
            Motion_LipsData = 0x0107,
            Motion_PlayE = 0x0108,
            Motion_PlaySE = 0x0109,
            Motion_PNum = 0x010A,
            #endregion

            #region Camera
            Camera_Position = 0x0200,
            Camera_Work = 0x0201,
            Camera_Hold = 0x0202,
            Camera_Vector = 0x0203,
            Camera_Mat = 0x0204,
            Camera_InteHold = 0x0205,
            Camera_InteVector = 0x0206,
            Camera_InteMat = 0x0207,
            Camera_Speed = 0x0208,
            Camera_Acceleration = 0x0209,
            Camera_IntePosition = 0x020A,
            Camera_InteSpeed = 0x020B,
            Camera_InteAcceleration = 0x020C,
            Camera_AngleSet = 0x020D,
            Camera_AngleSpeed = 0x020E,
            Camera_AngleAcceleration = 0x020F,
            Camera_LenSet = 0x0210,
            Camera_LenSpeed = 0x0211,
            Camera_LenAcceleration = 0x0212,
            Camera_MatG = 0x0213,
            Camera_InteMatG = 0x0214,
            Camera_MVP = 0x0215,
            Camera_InteMVP = 0x0216,
            Camera_VecG = 0x0217,
            Camera_InteVecG = 0x0218,
            Camera_LrSet = 0x0219,
            Camera_LrSpeed = 0x021A,
            Camera_LrAcceleration = 0x021B,
            Camera_LrMVP = 0x021C,
            Camera_InteLrSet = 0x021D,
            Camera_InteLrSpeed = 0x021E,
            Camera_InteLrAcceleration = 0x021F,
            Camera_InteLrMVP = 0x0220,
            Camera_LrMV = 0x0221,
            Camera_InteLrMV = 0x0222,
            Camera_MV = 0x0223,
            Camera_InteMV = 0x0224,
            Camera_AngleMV = 0x0225,
            Camera_AngleMVP = 0x0226,
            Camera_Speed1 = 0x0227,
            Camera_InteSpeed1 = 0x0228,
            Camera_LrSpeed1 = 0x0229,
            Camera_InteLrSpeed1 = 0x022A,
            Camera_AngleSpeed1 = 0x022B,
            Camera_Data = 0x022C,
            Camera_LenMVP = 0x022D,
            Camera_LenMV = 0x022E,
            Camera_ProjSet = 0x022F,
            Camera_ProjSpeed = 0x0230,
            Camera_ProjAcceleration = 0x0231,
            Camera_ProjMVP = 0x0232,
            Camera_ProjMV = 0x0233,
            Camera_MHP = 0x0234,
            Camera_InteMHP = 0x0235,
            Camera_LrMHP = 0x0236,
            Camera_InteLrMHP = 0x0237,
            Camera_LrMH = 0x0238,
            Camera_InteLrMH = 0x0239,
            Camera_MH = 0x023A,
            Camera_InteMH = 0x023B,
            Camera_AngleMH = 0x023C,
            Camera_AngleMHP = 0x023D,
            Camera_PNum = 0x023E,
            #endregion

            #region Game
            Game_RtData = 0x0300,
            Game_RtPosition = 0x0301,
            Game_RtSpeed = 0x0302,
            Game_RtAcceleration = 0x0303,
            Game_RtOff = 0x0304,
            Game_RtMV = 0x0305,
            Game_RtMVP = 0x0306,
            Game_StRt = 0x0307,
            Game_StSW = 0x0308,
            Game_RtWait = 0x0309,
            Game_Copy = 0x030A,
            Game_CamCat = 0x030B,
            Game_CamRel = 0x030C,
            Game_W2Rt = 0x030D,
            Game_Rt2W = 0x030E,
            Game_GameJ = 0x030F,
            Game_GameR = 0x0310,
            Game_Back = 0x0311,
            Game_PosJ = 0x0312,
            Game_Key = 0x0313,
            Game_RtJump = 0x0314,
            Game_Cent = 0x0315,
            Game_KeyJ = 0x0316,
            Game_Jump = 0x0317,
            Game_Shadow = 0x0318,
            Game_StSw2 = 0x0319,
            Game_Mir = 0x031A,
            Game_MesP = 0x031B,
            Game_MirN = 0x031C,
            Game_BackV = 0x031D,
            Game_BackY = 0x031E,
            Game_Switch = 0x031F,
            Game_VisionClear = 0x0320,
            Game_Reset = 0x0321,
            Game_NSkip = 0x0322,
            Game_LCol = 0x0323,
            Game_PNum = 0x0324,
            #endregion

            #region Sound
            Sound_Voice = 0x0400,
            Sound_SE = 0x0401,
            Sound_VoiceData = 0x0402,
            Sound_Selp = 0x0403,
            Sound_SelpOff = 0x0404,
            Sound_ABGM = 0x0405,
            Sound_NBGM = 0x0406,
            Sound_SPitSet = 0x0407,
            Sound_SPitMV = 0x0408,
            Sound_SPitMVP = 0x0409,
            Sound_SVolSet = 0x040A,
            Sound_SVolMV = 0x040B,
            Sound_SVolMVP = 0x040C,
            Sound_SEv = 0x040D,
            Sound_BGMSync = 0x040E,
            Sound_AC3J = 0x040F,
            Sound_AC3S = 0x0410,
            Sound_PNum = 0x0411,
            #endregion

            Invalid = 0x7FFF
        };
    }
}

