using BinarySerializer.PlayStation.PS2;

namespace BinarySerializer.Klonoa.LV
{
    public class LevelKazuyaPack_ArchiveFile : ArchiveFile
    {
        IconSys_File IconSys { get; set; }
        ICO_File BaseSaveIcon { get; set; }
        ICO_File CopySaveIcon { get; set; }
        ICO_File DeleteSaveIcon { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            IconSys = SerializeFile(s, IconSys, 0, name: nameof(IconSys));
            BaseSaveIcon = SerializeFile(s, BaseSaveIcon, 1, name: nameof(BaseSaveIcon));
            CopySaveIcon = SerializeFile(s, CopySaveIcon, 2, name: nameof(CopySaveIcon));
            DeleteSaveIcon = SerializeFile(s, DeleteSaveIcon, 3, name: nameof(DeleteSaveIcon));
        }
    }
}