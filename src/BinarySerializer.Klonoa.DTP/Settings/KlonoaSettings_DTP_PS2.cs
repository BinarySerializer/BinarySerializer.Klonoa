namespace BinarySerializer.Klonoa.DTP
{
    // TODO: Fully support loading the PS2 version. The loader needs to be updated to not rely on the IDX.
    public class KlonoaSettings_DTP_PS2 : KlonoaSettings_DTP
    {
        public override KlonoaGameVersion Version => KlonoaGameVersion.DTP_PS2;

        // MWo3 file. Contains code and data.
        public virtual string FilePath_EXE => "KLONOA.BIN";

        // TODO: The STM files match the data in the PS1 BIN file. The order is the same so the BIN index could be used here too.
        // We could also use these file names in the PS1 settings classes for nicer logging and exports? The file names seem to usually
        // match with the exceptions being the BOOT and TITLE folders.
        public (string FilePath, string Name, (string FileName, IDXLoadCommand.FileType FileType)[] Files)[] FileTable { get; } =
        {
            ("BOOT.STM", "BOOT", new []
            {
                ("BOOT.ARC", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("WMAPCLUT.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("GUAGOFF.ARC", IDXLoadCommand.FileType.FixedSprites),
                ("WMAP.DAT", IDXLoadCommand.FileType.Unknown), // TODO
                ("FIX.OAF", IDXLoadCommand.FileType.OA05),
                ("CLIPTBL.3D", IDXLoadCommand.FileType.Archive_ClipTable), // TODO
                ("PIG.ARC", IDXLoadCommand.FileType.Unknown), // TODO
            }),
            ("CDLG0.STM", "TITLE", new []
            {
                ("NAMCO_R.TIM", IDXLoadCommand.FileType.Unknown), // TODO: Single TIM file
                ("TITLE.OAF", IDXLoadCommand.FileType.OA05),
                ("TITLE.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("MEM.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("TABLETT.BIN", IDXLoadCommand.FileType.Code),
                ("TITLE.BIN", IDXLoadCommand.FileType.Code),
                ("MOVIE.BIN", IDXLoadCommand.FileType.Code),
                ("TITLE.OFF", IDXLoadCommand.FileType.Archive_MenuSprites),
                ("CDPLATE.ULZ", IDXLoadCommand.FileType.Archive_TIM_SongsText),
                ("FONT24.DAT", IDXLoadCommand.FileType.Font),
                ("MEM.ULZ", IDXLoadCommand.FileType.Archive_TIM_SaveText),
                ("BG8.TIA", IDXLoadCommand.FileType.Archive_MenuBackgrounds),
                ("MAP12.SEQ", IDXLoadCommand.FileType.SEQ),
                ("PS2MSG.TIA", IDXLoadCommand.FileType.Unknown), // TODO: PS2 only textures with text
            }),
            ("CDLG1.STM", "TITLE", new []
            {
                // Unknown names
                ("0", IDXLoadCommand.FileType.Code),
                ("1", IDXLoadCommand.FileType.Code),
                ("2", IDXLoadCommand.FileType.Code),
            }),

            // Areas
            ("CDAR11.STM", "AR11", new []
            {
                ("SND11.OAF", IDXLoadCommand.FileType.OA05),
                ("TPIC11.ARC", IDXLoadCommand.FileType.Archive_TIM_SpriteSheets),
                ("MAP11.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("BG11.BA", IDXLoadCommand.FileType.Archive_BackgroundPack),
                ("TABLE11.BIN", IDXLoadCommand.FileType.Code),
                ("AREA11.BIN", IDXLoadCommand.FileType.Code),
                ("TKDAT11.ARC", IDXLoadCommand.FileType.Archive_SpritePack),
                ("MAP11.NAK", IDXLoadCommand.FileType.Archive_LevelPack),
                ("MAP1.SEQ", IDXLoadCommand.FileType.SEQ),
            }),
            ("CDAR12.STM", "AR12", new []
            {
                ("SND12.OAF", IDXLoadCommand.FileType.OA05),
                ("TPIC12.ARC", IDXLoadCommand.FileType.Archive_TIM_SpriteSheets),
                ("MAP12.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("BG12.BA", IDXLoadCommand.FileType.Archive_BackgroundPack),
                ("MEM.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("TABLE12.BIN", IDXLoadCommand.FileType.Code),
                ("AREA12.BIN", IDXLoadCommand.FileType.Code),
                ("TKDAT12.ARC", IDXLoadCommand.FileType.Archive_SpritePack),
                ("MAP12.NAK", IDXLoadCommand.FileType.Archive_LevelPack),
                ("GMEM.OFF", IDXLoadCommand.FileType.Archive_LevelMenuSprites),
                ("MEM.ULZ", IDXLoadCommand.FileType.Archive_TIM_SaveText),
            }),
            ("CDAR13.STM", "AR13", new []
            {
                ("SND13.OAF", IDXLoadCommand.FileType.OA05),
                ("TPIC13.ARC", IDXLoadCommand.FileType.Archive_TIM_SpriteSheets),
                ("MAP13.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("BG13.BA", IDXLoadCommand.FileType.Archive_BackgroundPack),
                ("TABLE13.BIN", IDXLoadCommand.FileType.Code),
                ("AREA13.BIN", IDXLoadCommand.FileType.Code),
                ("SND13X.OAF", IDXLoadCommand.FileType.OA05),
                ("TKDAT13.ARC", IDXLoadCommand.FileType.Archive_SpritePack),
                ("MAP13.NAK", IDXLoadCommand.FileType.Archive_LevelPack),
                ("MAP2.SEQ", IDXLoadCommand.FileType.SEQ),
                // Unknown names
                ("10", IDXLoadCommand.FileType.Unknown), // TODO
                ("11", IDXLoadCommand.FileType.Unknown), // TODO
            }),
            ("CDAR21.STM", "AR21", new []
            {
                ("SND21.OAF", IDXLoadCommand.FileType.OA05),
                ("TPIC21.ARC", IDXLoadCommand.FileType.Archive_TIM_SpriteSheets),
                ("MAP21.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("BG21.BA", IDXLoadCommand.FileType.Archive_BackgroundPack),
                ("MEM.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("TABLE21.BIN", IDXLoadCommand.FileType.Code),
                ("AREA21.BIN", IDXLoadCommand.FileType.Code),
                ("TKDAT21.ARC", IDXLoadCommand.FileType.Archive_SpritePack),
                ("MAP21.NAK", IDXLoadCommand.FileType.Archive_LevelPack),
                ("GMEM.OFF", IDXLoadCommand.FileType.Archive_LevelMenuSprites),
                ("MEM.ULZ", IDXLoadCommand.FileType.Archive_TIM_SaveText),
                ("MAP3.SEQ", IDXLoadCommand.FileType.SEQ),
            }),
            ("CDAR22.STM", "AR22", new []
            {
                ("SND22.OAF", IDXLoadCommand.FileType.OA05),
                ("TPIC22.ARC", IDXLoadCommand.FileType.Archive_TIM_SpriteSheets),
                ("MAP22.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("BG22.BA", IDXLoadCommand.FileType.Archive_BackgroundPack),
                ("MEM.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("TABLE22.BIN", IDXLoadCommand.FileType.Code),
                ("AREA22.BIN", IDXLoadCommand.FileType.Code),
                ("TKDAT22.ARC", IDXLoadCommand.FileType.Archive_SpritePack),
                ("MAP22.NAK", IDXLoadCommand.FileType.Archive_LevelPack),
                ("GMEM.OFF", IDXLoadCommand.FileType.Archive_LevelMenuSprites),
                ("MEM.ULZ", IDXLoadCommand.FileType.Archive_TIM_SaveText),
            }),
            ("CDAR23.STM", "AR23", new []
            {
                ("SND23.OAF", IDXLoadCommand.FileType.OA05),
                ("TPIC23.ARC", IDXLoadCommand.FileType.Archive_TIM_SpriteSheets),
                ("MAP23.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("BG23.BA", IDXLoadCommand.FileType.Archive_BackgroundPack),
                ("TABLE23.BIN", IDXLoadCommand.FileType.Code),
                ("AREA23.BIN", IDXLoadCommand.FileType.Code),
                ("SND23X.OAF", IDXLoadCommand.FileType.OA05),
                ("TKDAT23.ARC", IDXLoadCommand.FileType.Archive_SpritePack),
                ("MAP23.NAK", IDXLoadCommand.FileType.Archive_LevelPack),
                ("MAP4.SEQ", IDXLoadCommand.FileType.SEQ),
                // Unknown names
                ("10", IDXLoadCommand.FileType.Unknown), // TODO
                ("11", IDXLoadCommand.FileType.Unknown), // TODO
            }),
            ("CDAR31.STM", "AR31", new []
            {
                ("SND31.OAF", IDXLoadCommand.FileType.OA05),
                ("TPIC31.ARC", IDXLoadCommand.FileType.Archive_TIM_SpriteSheets),
                ("MAP31.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("BG31.BA", IDXLoadCommand.FileType.Archive_BackgroundPack),
                ("MEM.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("TABLE31.BIN", IDXLoadCommand.FileType.Code),
                ("AREA31.BIN", IDXLoadCommand.FileType.Code),
                ("TKDAT31.ARC", IDXLoadCommand.FileType.Archive_SpritePack),
                ("MAP31.NAK", IDXLoadCommand.FileType.Archive_LevelPack),
                ("GMEM.OFF", IDXLoadCommand.FileType.Archive_LevelMenuSprites),
                ("MEM.ULZ", IDXLoadCommand.FileType.Archive_TIM_SaveText),
                ("MAP5.SEQ", IDXLoadCommand.FileType.SEQ),
            }),
            ("CDAR32.STM", "AR32", new []
            {
                ("SND32.OAF", IDXLoadCommand.FileType.OA05),
                ("TPIC32.ARC", IDXLoadCommand.FileType.Archive_TIM_SpriteSheets),
                ("MAP32.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("BG32.BA", IDXLoadCommand.FileType.Archive_BackgroundPack),
                ("MEM.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("TABLE32.BIN", IDXLoadCommand.FileType.Code),
                ("AREA32.BIN", IDXLoadCommand.FileType.Code),
                ("TKDAT32.ARC", IDXLoadCommand.FileType.Archive_SpritePack),
                ("MAP32.NAK", IDXLoadCommand.FileType.Archive_LevelPack),
                ("GMEM.OFF", IDXLoadCommand.FileType.Archive_LevelMenuSprites),
                ("MEM.ULZ", IDXLoadCommand.FileType.Archive_TIM_SaveText),
            }),
            ("CDAR33.STM", "AR33", new []
            {
                ("SND33.OAF", IDXLoadCommand.FileType.OA05),
                ("TPIC33.ARC", IDXLoadCommand.FileType.Archive_TIM_SpriteSheets),
                ("MAP33.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("BG33.BA", IDXLoadCommand.FileType.Archive_BackgroundPack),
                ("TABLE33.BIN", IDXLoadCommand.FileType.Code),
                ("AREA33.BIN", IDXLoadCommand.FileType.Code),
                ("TKDAT33.ARC", IDXLoadCommand.FileType.Archive_SpritePack),
                ("MAP33.NAK", IDXLoadCommand.FileType.Archive_LevelPack),
                ("MAP6.SEQ", IDXLoadCommand.FileType.SEQ),
                // Unknown names
                ("9", IDXLoadCommand.FileType.Unknown), // TODO
                ("10", IDXLoadCommand.FileType.Unknown), // TODO
            }),
            ("CDAR41.STM", "AR41", new []
            {
                ("SND41.OAF", IDXLoadCommand.FileType.OA05),
                ("TPIC41.ARC", IDXLoadCommand.FileType.Archive_TIM_SpriteSheets),
                ("MAP41.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("BG41.BA", IDXLoadCommand.FileType.Archive_BackgroundPack),
                ("MEM.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("TABLE41.BIN", IDXLoadCommand.FileType.Code),
                ("AREA41.BIN", IDXLoadCommand.FileType.Code),
                ("TKDAT41.ARC", IDXLoadCommand.FileType.Archive_SpritePack),
                ("MAP41.NAK", IDXLoadCommand.FileType.Archive_LevelPack),
                ("GMEM.OFF", IDXLoadCommand.FileType.Archive_LevelMenuSprites),
                ("MEM.ULZ", IDXLoadCommand.FileType.Archive_TIM_SaveText),
                ("MAP7.SEQ", IDXLoadCommand.FileType.SEQ),
            }),
            ("CDAR42.STM", "AR42", new []
            {
                ("SND42.OAF", IDXLoadCommand.FileType.OA05),
                ("TPIC42.ARC", IDXLoadCommand.FileType.Archive_TIM_SpriteSheets),
                ("MAP42.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("BG42.BA", IDXLoadCommand.FileType.Archive_BackgroundPack),
                ("MEM.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("TABLE42.BIN", IDXLoadCommand.FileType.Code),
                ("AREA42.BIN", IDXLoadCommand.FileType.Code),
                ("TKDAT42.ARC", IDXLoadCommand.FileType.Archive_SpritePack),
                ("MAP42.NAK", IDXLoadCommand.FileType.Archive_LevelPack),
                ("GMEM.OFF", IDXLoadCommand.FileType.Archive_LevelMenuSprites),
                ("MEM.ULZ", IDXLoadCommand.FileType.Archive_TIM_SaveText),
            }),
            ("CDAR43.STM", "AR43", new []
            {
                ("SND43.OAF", IDXLoadCommand.FileType.OA05),
                ("TPIC43.ARC", IDXLoadCommand.FileType.Archive_TIM_SpriteSheets),
                ("MAP43.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("BG43.BA", IDXLoadCommand.FileType.Archive_BackgroundPack),
                ("TABLE43.BIN", IDXLoadCommand.FileType.Code),
                ("AREA43.BIN", IDXLoadCommand.FileType.Code),
                ("SND43X.OAF", IDXLoadCommand.FileType.OA05),
                ("TKDAT43.ARC", IDXLoadCommand.FileType.Archive_SpritePack),
                ("MAP43.NAK", IDXLoadCommand.FileType.Archive_LevelPack),
                ("MAP8.SEQ", IDXLoadCommand.FileType.SEQ),
                // Unknown names
                ("10", IDXLoadCommand.FileType.Unknown), // TODO
                ("11", IDXLoadCommand.FileType.Unknown), // TODO
            }),
            ("CDAR51.STM", "AR51", new []
            {
                ("SND51.OAF", IDXLoadCommand.FileType.OA05),
                ("TPIC51.ARC", IDXLoadCommand.FileType.Archive_TIM_SpriteSheets),
                ("MAP51.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("BG51.BA", IDXLoadCommand.FileType.Archive_BackgroundPack),
                ("MEM.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("TABLE51.BIN", IDXLoadCommand.FileType.Code),
                ("AREA51.BIN", IDXLoadCommand.FileType.Code),
                ("TKDAT51.ARC", IDXLoadCommand.FileType.Archive_SpritePack),
                ("MAP51.NAK", IDXLoadCommand.FileType.Archive_LevelPack),
                ("GMEM.OFF", IDXLoadCommand.FileType.Archive_LevelMenuSprites),
                ("MEM.ULZ", IDXLoadCommand.FileType.Archive_TIM_SaveText),
                ("MAP9.SEQ", IDXLoadCommand.FileType.SEQ),
            }),
            ("CDAR52.STM", "AR52", new []
            {
                ("SND52.OAF", IDXLoadCommand.FileType.OA05),
                ("TPIC52.ARC", IDXLoadCommand.FileType.Archive_TIM_SpriteSheets),
                ("MAP52.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("BG52.BA", IDXLoadCommand.FileType.Archive_BackgroundPack),
                ("MEM.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("TABLE52.BIN", IDXLoadCommand.FileType.Code),
                ("AREA52.BIN", IDXLoadCommand.FileType.Code),
                ("BG52.CLT", IDXLoadCommand.FileType.BackgroundPalettes),
                ("TKDAT52.ARC", IDXLoadCommand.FileType.Archive_SpritePack),
                ("MAP52.NAK", IDXLoadCommand.FileType.Archive_LevelPack),
                ("GMEM.OFF", IDXLoadCommand.FileType.Archive_LevelMenuSprites),
                ("MEM.ULZ", IDXLoadCommand.FileType.Archive_TIM_SaveText),
            }),
            ("CDAR53.STM", "AR53", new []
            {
                ("SND53.OAF", IDXLoadCommand.FileType.OA05),
                ("TPIC53.ARC", IDXLoadCommand.FileType.Archive_TIM_SpriteSheets),
                ("MAP53.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("BG53.BA", IDXLoadCommand.FileType.Archive_BackgroundPack),
                ("TABLE53.BIN", IDXLoadCommand.FileType.Code),
                ("AREA53.BIN", IDXLoadCommand.FileType.Code),
                ("SND53X.OAF", IDXLoadCommand.FileType.OA05),
                ("BG53.CLT", IDXLoadCommand.FileType.BackgroundPalettes),
                ("TKDAT53.ARC", IDXLoadCommand.FileType.Archive_SpritePack),
                ("MOVIE.BIN", IDXLoadCommand.FileType.Code),
                ("MAP53.NAK", IDXLoadCommand.FileType.Archive_LevelPack),
                ("MAP10.SEQ", IDXLoadCommand.FileType.SEQ),
                // Unknown names
                ("12", IDXLoadCommand.FileType.Unknown), // TODO
                ("13", IDXLoadCommand.FileType.Unknown), // TODO
            }),
            ("CDAR61.STM", "AR61", new []
            {
                ("SND61.OAF", IDXLoadCommand.FileType.OA05),
                ("TPIC61.ARC", IDXLoadCommand.FileType.Archive_TIM_SpriteSheets),
                ("MAP61.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("BG61.BA", IDXLoadCommand.FileType.Archive_BackgroundPack),
                ("MEM.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("TABLE61.BIN", IDXLoadCommand.FileType.Code),
                ("AREA61.BIN", IDXLoadCommand.FileType.Code),
                ("TKDAT61.ARC", IDXLoadCommand.FileType.Archive_SpritePack),
                ("MAP61.NAK", IDXLoadCommand.FileType.Archive_LevelPack),
                ("GMEM.OFF", IDXLoadCommand.FileType.Archive_LevelMenuSprites),
                ("MEM.ULZ", IDXLoadCommand.FileType.Archive_TIM_SaveText),
                ("MAP11.SEQ", IDXLoadCommand.FileType.SEQ),
            }),
            ("CDAR62.STM", "AR62", new []
            {
                ("SND62.OAF", IDXLoadCommand.FileType.OA05),
                ("TPIC62.ARC", IDXLoadCommand.FileType.Archive_TIM_SpriteSheets),
                ("MAP62.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("BG62.BA", IDXLoadCommand.FileType.Archive_BackgroundPack),
                ("MEM.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("TABLE62.BIN", IDXLoadCommand.FileType.Code),
                ("AREA62.BIN", IDXLoadCommand.FileType.Code),
                ("TKDAT62.ARC", IDXLoadCommand.FileType.Archive_SpritePack),
                ("MAP62.NAK", IDXLoadCommand.FileType.Archive_LevelPack),
                ("GMEM.OFF", IDXLoadCommand.FileType.Archive_LevelMenuSprites),
                ("MEM.ULZ", IDXLoadCommand.FileType.Archive_TIM_SaveText),
            }),
            ("CDAR63.STM", "AR63", new []
            {
                ("SND63.OAF", IDXLoadCommand.FileType.OA05),
                ("TPIC63.ARC", IDXLoadCommand.FileType.Archive_TIM_SpriteSheets),
                ("MAP63.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("TEST63.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("BG63.BA", IDXLoadCommand.FileType.Archive_BackgroundPack),
                ("TABLE63.BIN", IDXLoadCommand.FileType.Code),
                ("AREA63.BIN", IDXLoadCommand.FileType.Code),
                ("TKDAT63.ARC", IDXLoadCommand.FileType.Archive_SpritePack),
                ("MAP63.NAK", IDXLoadCommand.FileType.Archive_LevelPack),
                ("MAP12.SEQ", IDXLoadCommand.FileType.SEQ),
                // Unknown names
                ("10", IDXLoadCommand.FileType.Unknown), // TODO
                ("11", IDXLoadCommand.FileType.Unknown), // TODO
            }),
            ("CDAR71.STM", "AR71", new []
            {
                ("SND71.OAF", IDXLoadCommand.FileType.OA05),
                ("TPIC71.ARC", IDXLoadCommand.FileType.Archive_TIM_SpriteSheets),
                ("MAP71.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("BG71.BA", IDXLoadCommand.FileType.Archive_BackgroundPack),
                ("TABLE71.BIN", IDXLoadCommand.FileType.Code),
                ("AREA71.BIN", IDXLoadCommand.FileType.Code),
                ("TKDAT71.ARC", IDXLoadCommand.FileType.Archive_SpritePack),
                ("MOVIE.BIN", IDXLoadCommand.FileType.Code),
                ("MAP71.NAK", IDXLoadCommand.FileType.Archive_LevelPack),
            }),
            ("CDAR72.STM", "AR72", new []
            {
                ("SND72.OAF", IDXLoadCommand.FileType.OA05),
                ("TPIC72.ARC", IDXLoadCommand.FileType.Archive_TIM_SpriteSheets),
                ("MAP72.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("BG72.BA", IDXLoadCommand.FileType.Archive_BackgroundPack),
                ("MEM.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("TABLE72.BIN", IDXLoadCommand.FileType.Code),
                ("AREA72.BIN", IDXLoadCommand.FileType.Code),
                ("TKDAT72.ARC", IDXLoadCommand.FileType.Archive_SpritePack),
                ("MAP72.NAK", IDXLoadCommand.FileType.Archive_LevelPack),
                ("GMEM.OFF", IDXLoadCommand.FileType.Archive_LevelMenuSprites),
                ("MEM.ULZ", IDXLoadCommand.FileType.Archive_TIM_SaveText),
            }),
            ("CDAR73.STM", "AR73", new []
            {
                ("SND73.OAF", IDXLoadCommand.FileType.OA05),
                ("TPIC73.ARC", IDXLoadCommand.FileType.Archive_TIM_SpriteSheets),
                ("MAP73.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("BG73.BA", IDXLoadCommand.FileType.Archive_BackgroundPack),
                ("TABLE73.BIN", IDXLoadCommand.FileType.Code),
                ("AREA73.BIN", IDXLoadCommand.FileType.Code),
                ("TKDAT73.ARC", IDXLoadCommand.FileType.Archive_SpritePack),
                ("MAP73.NAK", IDXLoadCommand.FileType.Archive_LevelPack),
                // Unknown names
                ("8", IDXLoadCommand.FileType.Unknown), // TODO
                ("9", IDXLoadCommand.FileType.Unknown), // TODO
            }),
            ("CDAR81.STM", "AR81", new []
            {
                ("SND81.OAF", IDXLoadCommand.FileType.OA05),
                ("TPIC81.ARC", IDXLoadCommand.FileType.Archive_TIM_SpriteSheets),
                ("MAP81.TIA", IDXLoadCommand.FileType.Archive_TIM_Generic),
                ("BG81.BA", IDXLoadCommand.FileType.Archive_BackgroundPack),
                ("TABLE81.BIN", IDXLoadCommand.FileType.Code),
                ("AREA81.BIN", IDXLoadCommand.FileType.Code),
                ("TKDAT81.ARC", IDXLoadCommand.FileType.Archive_SpritePack),
                ("MAP81.NAK", IDXLoadCommand.FileType.Archive_LevelPack),
                // Unknown names
                ("8", IDXLoadCommand.FileType.Unknown), // TODO
                ("9", IDXLoadCommand.FileType.Unknown), // TODO
            }),
        };
    } 
}