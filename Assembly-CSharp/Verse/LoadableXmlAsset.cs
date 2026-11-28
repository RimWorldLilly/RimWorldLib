using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;

namespace Verse
{
	// Token: 0x02000514 RID: 1300
	public class LoadableXmlAsset
	{
		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x06002688 RID: 9864 RVA: 0x000EE5C8 File Offset: 0x000EC7C8
		public string FullFilePath
		{
			get
			{
				ReadOnlySpan<char> str = this.fullFolderPath;
				char directorySeparatorChar = Path.DirectorySeparatorChar;
				return str + new ReadOnlySpan<char>(ref directorySeparatorChar) + this.name;
			}
		}

		// Token: 0x06002689 RID: 9865 RVA: 0x000EE600 File Offset: 0x000EC800
		public LoadableXmlAsset(string name, string text)
		{
			this.name = name;
			this.fullFolderPath = string.Empty;
			try
			{
				using (StringReader stringReader = new StringReader(text))
				{
					using (XmlReader xmlReader = XmlReader.Create(stringReader, LoadableXmlAsset.Settings))
					{
						this.xmlDoc = new XmlDocument();
						this.xmlDoc.Load(xmlReader);
					}
				}
			}
			catch (Exception value)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Exception reading ");
				defaultInterpolatedStringHandler.AppendFormatted(name);
				defaultInterpolatedStringHandler.AppendLiteral(" as XML: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
				Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				this.xmlDoc = null;
			}
		}

		// Token: 0x0600268A RID: 9866 RVA: 0x000EE6DC File Offset: 0x000EC8DC
		public unsafe LoadableXmlAsset(FileInfo file, ModContentPack mod)
		{
			this.mod = mod;
			this.name = file.Name;
			this.fullFolderPath = file.Directory.FullName;
			try
			{
				using (MemoryMappedFileSpanWrapper memoryMappedFileSpanWrapper = new MemoryMappedFileSpanWrapper(file))
				{
					ReadOnlySpan<byte> bytes = memoryMappedFileSpanWrapper.GetReadOnlySpan(0L, memoryMappedFileSpanWrapper.FileSize);
					if (bytes.Length >= 3 && *bytes[0] == 239 && *bytes[1] == 187 && *bytes[2] == 191)
					{
						bytes = bytes.Slice(3);
					}
					using (StringReader stringReader = new StringReader(Encoding.UTF8.GetString(bytes)))
					{
						using (XmlReader xmlReader = XmlReader.Create(stringReader, LoadableXmlAsset.Settings))
						{
							this.xmlDoc = new XmlDocument();
							this.xmlDoc.Load(xmlReader);
						}
					}
				}
			}
			catch (Exception value)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Exception reading ");
				defaultInterpolatedStringHandler.AppendFormatted(this.name);
				defaultInterpolatedStringHandler.AppendLiteral(" as XML: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
				Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				this.xmlDoc = null;
			}
		}

		// Token: 0x0600268B RID: 9867 RVA: 0x000EE84C File Offset: 0x000ECA4C
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x04001BC2 RID: 7106
		public readonly string name;

		// Token: 0x04001BC3 RID: 7107
		public readonly string fullFolderPath;

		// Token: 0x04001BC4 RID: 7108
		public readonly XmlDocument xmlDoc;

		// Token: 0x04001BC5 RID: 7109
		public readonly ModContentPack mod;

		// Token: 0x04001BC6 RID: 7110
		private const int BufferSize = 16000;

		// Token: 0x04001BC7 RID: 7111
		private static readonly XmlReaderSettings Settings = new XmlReaderSettings
		{
			IgnoreComments = true,
			IgnoreWhitespace = true,
			CheckCharacters = false,
			Async = false
		};
	}
}
