// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: variation_text.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace SeppoService {

  /// <summary>Holder for reflection information generated from variation_text.proto</summary>
  public static partial class VariationTextReflection {

    #region Descriptor
    /// <summary>File descriptor for variation_text.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static VariationTextReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChR2YXJpYXRpb25fdGV4dC5wcm90bxIMU2VwcG9TZXJ2aWNlIj4KDVZhcmlh",
            "dGlvblRleHQSCgoCaWQYASABKA0SEwoLdmFyaWF0aW9uSWQYAiABKA0SDAoE",
            "dGV4dBgDIAEoCSI+CiZGZXRjaFZhcmlhdGlvblRleHRCeVZhcmlhdGlvbklk",
            "UmVxdWVzdBIUCgx2YXJpYXRpb25JZHMYASADKA0iXgonRmV0Y2hWYXJpYXRp",
            "b25UZXh0QnlWYXJpYXRpb25JZFJlc3BvbnNlEjMKDnZhcmlhdGlvblRleHRz",
            "GAEgAygLMhsuU2VwcG9TZXJ2aWNlLlZhcmlhdGlvblRleHRiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::SeppoService.VariationText), global::SeppoService.VariationText.Parser, new[]{ "Id", "VariationId", "Text" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::SeppoService.FetchVariationTextByVariationIdRequest), global::SeppoService.FetchVariationTextByVariationIdRequest.Parser, new[]{ "VariationIds" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::SeppoService.FetchVariationTextByVariationIdResponse), global::SeppoService.FetchVariationTextByVariationIdResponse.Parser, new[]{ "VariationTexts" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class VariationText : pb::IMessage<VariationText> {
    private static readonly pb::MessageParser<VariationText> _parser = new pb::MessageParser<VariationText>(() => new VariationText());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<VariationText> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SeppoService.VariationTextReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public VariationText() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public VariationText(VariationText other) : this() {
      id_ = other.id_;
      variationId_ = other.variationId_;
      text_ = other.text_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public VariationText Clone() {
      return new VariationText(this);
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

    /// <summary>Field number for the "variationId" field.</summary>
    public const int VariationIdFieldNumber = 2;
    private uint variationId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint VariationId {
      get { return variationId_; }
      set {
        variationId_ = value;
      }
    }

    /// <summary>Field number for the "text" field.</summary>
    public const int TextFieldNumber = 3;
    private string text_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Text {
      get { return text_; }
      set {
        text_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as VariationText);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(VariationText other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Id != other.Id) return false;
      if (VariationId != other.VariationId) return false;
      if (Text != other.Text) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Id != 0) hash ^= Id.GetHashCode();
      if (VariationId != 0) hash ^= VariationId.GetHashCode();
      if (Text.Length != 0) hash ^= Text.GetHashCode();
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
      if (VariationId != 0) {
        output.WriteRawTag(16);
        output.WriteUInt32(VariationId);
      }
      if (Text.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(Text);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Id != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(Id);
      }
      if (VariationId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(VariationId);
      }
      if (Text.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Text);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(VariationText other) {
      if (other == null) {
        return;
      }
      if (other.Id != 0) {
        Id = other.Id;
      }
      if (other.VariationId != 0) {
        VariationId = other.VariationId;
      }
      if (other.Text.Length != 0) {
        Text = other.Text;
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
            VariationId = input.ReadUInt32();
            break;
          }
          case 26: {
            Text = input.ReadString();
            break;
          }
        }
      }
    }

  }

  public sealed partial class FetchVariationTextByVariationIdRequest : pb::IMessage<FetchVariationTextByVariationIdRequest> {
    private static readonly pb::MessageParser<FetchVariationTextByVariationIdRequest> _parser = new pb::MessageParser<FetchVariationTextByVariationIdRequest>(() => new FetchVariationTextByVariationIdRequest());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<FetchVariationTextByVariationIdRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SeppoService.VariationTextReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FetchVariationTextByVariationIdRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FetchVariationTextByVariationIdRequest(FetchVariationTextByVariationIdRequest other) : this() {
      variationIds_ = other.variationIds_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FetchVariationTextByVariationIdRequest Clone() {
      return new FetchVariationTextByVariationIdRequest(this);
    }

    /// <summary>Field number for the "variationIds" field.</summary>
    public const int VariationIdsFieldNumber = 1;
    private static readonly pb::FieldCodec<uint> _repeated_variationIds_codec
        = pb::FieldCodec.ForUInt32(10);
    private readonly pbc::RepeatedField<uint> variationIds_ = new pbc::RepeatedField<uint>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<uint> VariationIds {
      get { return variationIds_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as FetchVariationTextByVariationIdRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(FetchVariationTextByVariationIdRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!variationIds_.Equals(other.variationIds_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= variationIds_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      variationIds_.WriteTo(output, _repeated_variationIds_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += variationIds_.CalculateSize(_repeated_variationIds_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(FetchVariationTextByVariationIdRequest other) {
      if (other == null) {
        return;
      }
      variationIds_.Add(other.variationIds_);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 10:
          case 8: {
            variationIds_.AddEntriesFrom(input, _repeated_variationIds_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class FetchVariationTextByVariationIdResponse : pb::IMessage<FetchVariationTextByVariationIdResponse> {
    private static readonly pb::MessageParser<FetchVariationTextByVariationIdResponse> _parser = new pb::MessageParser<FetchVariationTextByVariationIdResponse>(() => new FetchVariationTextByVariationIdResponse());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<FetchVariationTextByVariationIdResponse> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SeppoService.VariationTextReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FetchVariationTextByVariationIdResponse() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FetchVariationTextByVariationIdResponse(FetchVariationTextByVariationIdResponse other) : this() {
      variationTexts_ = other.variationTexts_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FetchVariationTextByVariationIdResponse Clone() {
      return new FetchVariationTextByVariationIdResponse(this);
    }

    /// <summary>Field number for the "variationTexts" field.</summary>
    public const int VariationTextsFieldNumber = 1;
    private static readonly pb::FieldCodec<global::SeppoService.VariationText> _repeated_variationTexts_codec
        = pb::FieldCodec.ForMessage(10, global::SeppoService.VariationText.Parser);
    private readonly pbc::RepeatedField<global::SeppoService.VariationText> variationTexts_ = new pbc::RepeatedField<global::SeppoService.VariationText>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::SeppoService.VariationText> VariationTexts {
      get { return variationTexts_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as FetchVariationTextByVariationIdResponse);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(FetchVariationTextByVariationIdResponse other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!variationTexts_.Equals(other.variationTexts_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= variationTexts_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      variationTexts_.WriteTo(output, _repeated_variationTexts_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += variationTexts_.CalculateSize(_repeated_variationTexts_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(FetchVariationTextByVariationIdResponse other) {
      if (other == null) {
        return;
      }
      variationTexts_.Add(other.variationTexts_);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 10: {
            variationTexts_.AddEntriesFrom(input, _repeated_variationTexts_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
