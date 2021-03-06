﻿using System.Collections.Generic;
using System.Linq;
using VsChromium.Core.Ipc.TypedMessages;
using VsChromium.Server.FileSystemNames;

namespace VsChromium.Server.FileSystemSnapshot {
  public static class FileSystemSnapshotExtensions {
    public static  FileSystemTree ToIpcFileSystemTree(this FileSystemTreeSnapshot tree) {
      return new FileSystemTree {
        Version = tree.Version,
        Root = BuildFileSystemTreeRoot(tree)
      };
    }

    private static DirectoryEntry BuildFileSystemTreeRoot(FileSystemTreeSnapshot fileSystemSnapshot) {
      return new DirectoryEntry {
        Name = null,
        Data = null,
        Entries = fileSystemSnapshot.ProjectRoots.Select(x => BuildDirectoryEntry(x.Directory)).Cast<FileSystemEntry>().ToList()
      };
    }

    private static DirectoryEntry BuildDirectoryEntry(DirectorySnapshot directoryEntry) {
      return new DirectoryEntry {
        Name = (directoryEntry.DirectoryName.IsAbsoluteName ? directoryEntry.DirectoryName.FullPath.Value : directoryEntry.DirectoryName.RelativePath.FileName),
        Data = null,
        Entries = BuildEntries(directoryEntry)
      };
    }

    private static FileSystemEntry BuildFileEntry(FileName filename) {
      return new FileEntry {
        Name = filename.RelativePath.FileName,
        Data = null
      };
    }
    private static List<FileSystemEntry> BuildEntries(DirectorySnapshot directoryEntry) {
      return directoryEntry.ChildDirectories
        .Select(x => BuildDirectoryEntry(x))
        .Concat(directoryEntry.ChildFiles.Select(x => BuildFileEntry(x)))
        .ToList();
    }

  }
}