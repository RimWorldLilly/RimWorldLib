using System;
using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;
using RimWorld.IO;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000339 RID: 825
	public static class ModDdsLoader
	{
		// Token: 0x06001940 RID: 6464 RVA: 0x000975D4 File Offset: 0x000957D4
		public unsafe static Texture2D TryLoadDds(VirtualFile file)
		{
			FilesystemFile filesystemFile = file as FilesystemFile;
			if (filesystemFile == null)
			{
				throw new NotSupportedException("ModDdsLoader only supports FilesystemFile types.");
			}
			FileInfo fileInfo = new FileInfo(filesystemFile.FullPath);
			Texture2D texture2D = Texture2D.LoadFromTextureCache(fileInfo.FullName);
			if (texture2D != null)
			{
				texture2D.name = Path.GetFileNameWithoutExtension(file.Name);
				texture2D.filterMode = FilterMode.Trilinear;
				texture2D.anisoLevel = 0;
				return texture2D;
			}
			Texture2D result;
			using (MemoryMappedFileSpanWrapper memoryMappedFileSpanWrapper = new MemoryMappedFileSpanWrapper(fileInfo, true))
			{
				DdsHeader ddsHeader = memoryMappedFileSpanWrapper.Read<DdsHeader>(0L);
				if (ddsHeader.Magic != 542327876U)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Invalid DDS magic number: ");
					defaultInterpolatedStringHandler.AppendFormatted<uint>(ddsHeader.Magic, "X8");
					defaultInterpolatedStringHandler.AppendLiteral(". Expected: ");
					defaultInterpolatedStringHandler.AppendFormatted<uint>(542327876U, "X8");
					throw new InvalidDataException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				if (ddsHeader.Size != 124U)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(37, 2);
					defaultInterpolatedStringHandler2.AppendLiteral("Invalid DDS header size: ");
					defaultInterpolatedStringHandler2.AppendFormatted<uint>(ddsHeader.Size);
					defaultInterpolatedStringHandler2.AppendLiteral(". Expected: ");
					defaultInterpolatedStringHandler2.AppendFormatted<uint>(124U);
					throw new InvalidDataException(defaultInterpolatedStringHandler2.ToStringAndClear());
				}
				if (ddsHeader.PixelFormat.Size != 32U)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(43, 2);
					defaultInterpolatedStringHandler3.AppendLiteral("Invalid DDS pixel format size: ");
					defaultInterpolatedStringHandler3.AppendFormatted<uint>(ddsHeader.PixelFormat.Size);
					defaultInterpolatedStringHandler3.AppendLiteral(". Expected: ");
					defaultInterpolatedStringHandler3.AppendFormatted<uint>(32U);
					throw new InvalidDataException(defaultInterpolatedStringHandler3.ToStringAndClear());
				}
				TextureFormat textureFormat = ddsHeader.PixelFormat.ToTextureFormat();
				int num = (textureFormat == TextureFormat.BC7) ? 148 : 128;
				if (ddsHeader.PixelFormat.IsBgr888 && !ddsHeader.PixelFormat.IsCompressed)
				{
					Span<byte> span = memoryMappedFileSpanWrapper.GetSpan((long)num);
					byte[] array = ArrayPool<byte>.Shared.Rent(span.Length);
					Span<byte> span2 = new Span<byte>(array, 0, span.Length);
					span.CopyTo(span2);
					int num2 = (int)(ddsHeader.PixelFormat.RGBBitCount / 8U);
					for (int i = 0; i < span.Length; i += num2)
					{
						ref byte ptr = ref span2[i];
						ref byte ptr2 = ref span2[i + 2];
						byte b = *span2[i + 2];
						byte b2 = *span2[i];
						ptr = b;
						ptr2 = b2;
					}
					Texture2D texture2D2 = ModDdsLoader.CreateTexture(file, ddsHeader, textureFormat, span2);
					ArrayPool<byte>.Shared.Return(array, false);
					result = texture2D2;
				}
				else
				{
					result = ModDdsLoader.CreateTexture(file, ddsHeader, textureFormat, memoryMappedFileSpanWrapper.GetSpan((long)num));
				}
			}
			return result;
		}

		// Token: 0x06001941 RID: 6465 RVA: 0x0009787C File Offset: 0x00095A7C
		private unsafe static Texture2D CreateTexture(VirtualFile file, DdsHeader header, TextureFormat format, Span<byte> data)
		{
			bool flag = (header.Flags & DdsHeaderFlags.MipMapCount) != (DdsHeaderFlags)0U && header.MipMapCount > 1U;
			int mipCount = (int)(flag ? header.MipMapCount : 1U);
			Texture2D texture2D = new Texture2D((int)header.Width, (int)header.Height, format, mipCount, false, true);
			fixed (byte* pinnableReference = data.GetPinnableReference())
			{
				byte* data2 = pinnableReference;
				texture2D.LoadRawTextureData(data2, data.Length);
			}
			texture2D.PersistToTextureCache(file.FullPath);
			texture2D.name = Path.GetFileNameWithoutExtension(file.Name);
			texture2D.filterMode = FilterMode.Trilinear;
			texture2D.anisoLevel = 0;
			texture2D.Apply(!flag, true);
			return texture2D;
		}
	}
}
