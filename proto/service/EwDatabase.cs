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
            "b3RvIgwKCkV3RGF0YWJhc2UiUwoVU3luY0V3RGF0YWJhc2VSZXF1ZXN0EhQK",
            "DEV3RGF0YWJhc2VJZBgBIAEoDRIkCgZld1NvbmcYAiADKAsyFC5TZXBwb1Nl",
            "cnZpY2UuRXdTb25nIhgKFlN5bmNFd0RhdGFiYXNlUmVzcG9uc2UiNQodTGlz",
            "dGVuRm9yQ2hhbmdlZEV3U29uZ1JlcXVlc3QSFAoMZXdEYXRhYmFzZUlkGAEg",
            "ASgNYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::SeppoService.EwSongReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::SeppoService.EwDatabase), global::SeppoService.EwDatabase.Parser, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::SeppoService.SyncEwDatabaseRequest), global::SeppoService.SyncEwDatabaseRequest.Parser, new[]{ "EwDatabaseId", "EwSong" }, null, null, null),
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
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public EwDatabase Clone() {
      return new EwDatabase(this);
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
    public void MergeFrom(EwDatabase other) {
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
      ewSong_ = other.ewSong_.Clone();
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

    /// <summary>Field number for the "ewSong" field.</summary>
    public const int EwSongFieldNumber = 2;
    private static readonly pb::FieldCodec<global::SeppoService.EwSong> _repeated_ewSong_codec
        = pb::FieldCodec.ForMessage(18, global::SeppoService.EwSong.Parser);
    private readonly pbc::RepeatedField<global::SeppoService.EwSong> ewSong_ = new pbc::RepeatedField<global::SeppoService.EwSong>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::SeppoService.EwSong> EwSong {
      get { return ewSong_; }
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
      if(!ewSong_.Equals(other.ewSong_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (EwDatabaseId != 0) hash ^= EwDatabaseId.GetHashCode();
      hash ^= ewSong_.GetHashCode();
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
      ewSong_.WriteTo(output, _repeated_ewSong_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (EwDatabaseId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(EwDatabaseId);
      }
      size += ewSong_.CalculateSize(_repeated_ewSong_codec);
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
      ewSong_.Add(other.ewSong_);
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
            ewSong_.AddEntriesFrom(input, _repeated_ewSong_codec);
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
