// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: ew_database.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace SeppoService {

  /// <summary>Holder for reflection information generated from ew_database.proto</summary>
  public static partial class EwDatabaseReflection {

    #region Descriptor
    /// <summary>File descriptor for ew_database.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static EwDatabaseReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChFld19kYXRhYmFzZS5wcm90bxIMU2VwcG9TZXJ2aWNlGg1ld19zb25nLnBy",
            "b3RvIjcKCkV3RGF0YWJhc2USCgoCaWQYASABKA0SDAoEZGJJZBgCIAEoDRIP",
            "Cgd2ZXJzaW9uGAMgASgEIlQKFVN5bmNFd0RhdGFiYXNlUmVxdWVzdBIUCgxF",
            "d0RhdGFiYXNlSWQYASABKA0SJQoHZXdTb25ncxgCIAMoCzIULlNlcHBvU2Vy",
            "dmljZS5Fd1NvbmciGAoWU3luY0V3RGF0YWJhc2VSZXNwb25zZSI1Ch1MaXN0",
            "ZW5Gb3JDaGFuZ2VkRXdTb25nUmVxdWVzdBIUCgxld0RhdGFiYXNlSWQYASAB",
            "KA1iBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::SeppoService.EwSongReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::SeppoService.EwDatabase), global::SeppoService.EwDatabase.Parser, new[]{ "Id", "DbId", "Version" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::SeppoService.SyncEwDatabaseRequest), global::SeppoService.SyncEwDatabaseRequest.Parser, new[]{ "EwDatabaseId", "EwSongs" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::SeppoService.SyncEwDatabaseResponse), global::SeppoService.SyncEwDatabaseResponse.Parser, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::SeppoService.ListenForChangedEwSongRequest), global::SeppoService.ListenForChangedEwSongRequest.Parser, new[]{ "EwDatabaseId" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class EwDatabase : pb::IMessage<EwDatabase> {
    private static readonly pb::MessageParser<EwDatabase> _parser = new pb::MessageParser<EwDatabase>(() => new EwDatabase());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<EwDatabase> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SeppoService.EwDatabaseReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public EwDatabase() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public EwDatabase(EwDatabase other) : this() {
      id_ = other.id_;
      dbId_ = other.dbId_;
      version_ = other.version_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public EwDatabase Clone() {
      return new EwDatabase(this);
    }

    /// <summary>Field number for the "id" field.</summary>
    public const int IdFieldNumber = 1;
    private uint id_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint Id {
      get { return id_; }
      set {
        id_ = value;
      }
    }

    /// <summary>Field number for the "dbId" field.</summary>
    public const int DbIdFieldNumber = 2;
    private uint dbId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint DbId {
      get { return dbId_; }
      set {
        dbId_ = value;
      }
    }

    /// <summary>Field number for the "version" field.</summary>
    public const int VersionFieldNumber = 3;
    private ulong version_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ulong Version {
      get { return version_; }
      set {
        version_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as EwDatabase);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(EwDatabase other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Id != other.Id) return false;
      if (DbId != other.DbId) return false;
      if (Version != other.Version) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Id != 0) hash ^= Id.GetHashCode();
      if (DbId != 0) hash ^= DbId.GetHashCode();
      if (Version != 0UL) hash ^= Version.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Id != 0) {
        output.WriteRawTag(8);
        output.WriteUInt32(Id);
      }
      if (DbId != 0) {
        output.WriteRawTag(16);
        output.WriteUInt32(DbId);
      }
      if (Version != 0UL) {
        output.WriteRawTag(24);
        output.WriteUInt64(Version);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Id != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(Id);
      }
      if (DbId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(DbId);
      }
      if (Version != 0UL) {
        size += 1 + pb::CodedOutputStream.ComputeUInt64Size(Version);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(EwDatabase other) {
      if (other == null) {
        return;
      }
      if (other.Id != 0) {
        Id = other.Id;
      }
      if (other.DbId != 0) {
        DbId = other.DbId;
      }
      if (other.Version != 0UL) {
        Version = other.Version;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            Id = input.ReadUInt32();
            break;
          }
          case 16: {
            DbId = input.ReadUInt32();
            break;
          }
          case 24: {
            Version = input.ReadUInt64();
            break;
          }
        }
      }
    }

  }

  public sealed partial class SyncEwDatabaseRequest : pb::IMessage<SyncEwDatabaseRequest> {
    private static readonly pb::MessageParser<SyncEwDatabaseRequest> _parser = new pb::MessageParser<SyncEwDatabaseRequest>(() => new SyncEwDatabaseRequest());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<SyncEwDatabaseRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SeppoService.EwDatabaseReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SyncEwDatabaseRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SyncEwDatabaseRequest(SyncEwDatabaseRequest other) : this() {
      ewDatabaseId_ = other.ewDatabaseId_;
      ewSongs_ = other.ewSongs_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SyncEwDatabaseRequest Clone() {
      return new SyncEwDatabaseRequest(this);
    }

    /// <summary>Field number for the "EwDatabaseId" field.</summary>
    public const int EwDatabaseIdFieldNumber = 1;
    private uint ewDatabaseId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint EwDatabaseId {
      get { return ewDatabaseId_; }
      set {
        ewDatabaseId_ = value;
      }
    }

    /// <summary>Field number for the "ewSongs" field.</summary>
    public const int EwSongsFieldNumber = 2;
    private static readonly pb::FieldCodec<global::SeppoService.EwSong> _repeated_ewSongs_codec
        = pb::FieldCodec.ForMessage(18, global::SeppoService.EwSong.Parser);
    private readonly pbc::RepeatedField<global::SeppoService.EwSong> ewSongs_ = new pbc::RepeatedField<global::SeppoService.EwSong>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::SeppoService.EwSong> EwSongs {
      get { return ewSongs_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as SyncEwDatabaseRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(SyncEwDatabaseRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (EwDatabaseId != other.EwDatabaseId) return false;
      if(!ewSongs_.Equals(other.ewSongs_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (EwDatabaseId != 0) hash ^= EwDatabaseId.GetHashCode();
      hash ^= ewSongs_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (EwDatabaseId != 0) {
        output.WriteRawTag(8);
        output.WriteUInt32(EwDatabaseId);
      }
      ewSongs_.WriteTo(output, _repeated_ewSongs_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (EwDatabaseId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(EwDatabaseId);
      }
      size += ewSongs_.CalculateSize(_repeated_ewSongs_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(SyncEwDatabaseRequest other) {
      if (other == null) {
        return;
      }
      if (other.EwDatabaseId != 0) {
        EwDatabaseId = other.EwDatabaseId;
      }
      ewSongs_.Add(other.ewSongs_);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            EwDatabaseId = input.ReadUInt32();
            break;
          }
          case 18: {
            ewSongs_.AddEntriesFrom(input, _repeated_ewSongs_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class SyncEwDatabaseResponse : pb::IMessage<SyncEwDatabaseResponse> {
    private static readonly pb::MessageParser<SyncEwDatabaseResponse> _parser = new pb::MessageParser<SyncEwDatabaseResponse>(() => new SyncEwDatabaseResponse());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<SyncEwDatabaseResponse> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SeppoService.EwDatabaseReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SyncEwDatabaseResponse() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SyncEwDatabaseResponse(SyncEwDatabaseResponse other) : this() {
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SyncEwDatabaseResponse Clone() {
      return new SyncEwDatabaseResponse(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as SyncEwDatabaseResponse);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(SyncEwDatabaseResponse other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(SyncEwDatabaseResponse other) {
      if (other == null) {
        return;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
        }
      }
    }

  }

  public sealed partial class ListenForChangedEwSongRequest : pb::IMessage<ListenForChangedEwSongRequest> {
    private static readonly pb::MessageParser<ListenForChangedEwSongRequest> _parser = new pb::MessageParser<ListenForChangedEwSongRequest>(() => new ListenForChangedEwSongRequest());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ListenForChangedEwSongRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SeppoService.EwDatabaseReflection.Descriptor.MessageTypes[3]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ListenForChangedEwSongRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ListenForChangedEwSongRequest(ListenForChangedEwSongRequest other) : this() {
      ewDatabaseId_ = other.ewDatabaseId_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ListenForChangedEwSongRequest Clone() {
      return new ListenForChangedEwSongRequest(this);
    }

    /// <summary>Field number for the "ewDatabaseId" field.</summary>
    public const int EwDatabaseIdFieldNumber = 1;
    private uint ewDatabaseId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint EwDatabaseId {
      get { return ewDatabaseId_; }
      set {
        ewDatabaseId_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ListenForChangedEwSongRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ListenForChangedEwSongRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (EwDatabaseId != other.EwDatabaseId) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (EwDatabaseId != 0) hash ^= EwDatabaseId.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (EwDatabaseId != 0) {
        output.WriteRawTag(8);
        output.WriteUInt32(EwDatabaseId);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (EwDatabaseId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(EwDatabaseId);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ListenForChangedEwSongRequest other) {
      if (other == null) {
        return;
      }
      if (other.EwDatabaseId != 0) {
        EwDatabaseId = other.EwDatabaseId;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            EwDatabaseId = input.ReadUInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
