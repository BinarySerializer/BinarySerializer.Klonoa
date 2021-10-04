# File

Commonly used in DTP and LV is a hierarchical data structure. In this library these are defined as files. If a file contains other files it's an archive.

**Archive**

A collection of files. An archive is defined by having a header with offsets leading to each file it contains. Although there is no defined length for each file we can determine it using all the available offsets (taking data alignment into account).

When an archive doesn't have any files it has the count set to -1. Multiple file offsets can point to the same file, although this is rare and mostly used for dummy data.

The order of each file is very important. The game parses these archives in two ways. The first is where each file is indexed at a specific place. The second way is where each file is of the same type and the game enumerated through it (easily handled with the generic `ArchiveFile<T>`).

It is worth noting an archive can contain multiple archives! Because of this `ArchiveFile` inherits from `BaseFile`.

**File**

A file is any data type which is contained in an archive. Parsing a file from an archive will automatically fill out its size and decompress it if it's compressed.