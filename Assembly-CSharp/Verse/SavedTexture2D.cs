using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004F5 RID: 1269
	public class SavedTexture2D : IExposable, IDisposable
	{
		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x060025A5 RID: 9637 RVA: 0x000E5138 File Offset: 0x000E3338
		// (set) Token: 0x060025A6 RID: 9638 RVA: 0x000E5140 File Offset: 0x000E3340
		public Texture2D Texture { get; private set; }

		// Token: 0x060025A7 RID: 9639 RVA: 0x000E5149 File Offset: 0x000E3349
		public SavedTexture2D(Texture2D texture)
		{
			this.Texture = texture;
		}

		// Token: 0x060025A8 RID: 9640 RVA: 0x000E5158 File Offset: 0x000E3358
		public void ExposeData()
		{
			int width = 0;
			int height = 0;
			byte textureFormat = 0;
			int mipCount = -1;
			bool linear = false;
			string name = "";
			int anisoLevel = 0;
			byte filterMode = 0;
			byte wrapModeU = 0;
			byte wrapModeV = 0;
			float mipMapBias = 0f;
			int requestedMipmapLevel = 0;
			bool ignoreMipmapLimit = false;
			int minimumMipmapLevel = 0;
			byte[] data = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				width = this.Texture.width;
				height = this.Texture.height;
				textureFormat = (byte)this.Texture.format;
				mipCount = this.Texture.mipmapCount;
				linear = !this.Texture.isDataSRGB;
				name = this.Texture.name;
				anisoLevel = this.Texture.anisoLevel;
				filterMode = (byte)this.Texture.filterMode;
				wrapModeU = (byte)this.Texture.wrapModeU;
				wrapModeV = (byte)this.Texture.wrapModeV;
				mipMapBias = this.Texture.mipMapBias;
				requestedMipmapLevel = this.Texture.requestedMipmapLevel;
				ignoreMipmapLimit = this.Texture.ignoreMipmapLimit;
				minimumMipmapLevel = this.Texture.minimumMipmapLevel;
				data = this.Texture.GetRawTextureData();
			}
			Scribe_Values.Look<string>(ref name, "name", null, false);
			Scribe_Values.Look<int>(ref width, "width", 0, false);
			Scribe_Values.Look<int>(ref height, "height", 0, false);
			Scribe_Values.Look<byte>(ref textureFormat, "format", 0, false);
			Scribe_Values.Look<int>(ref mipCount, "mipCount", 0, false);
			Scribe_Values.Look<bool>(ref linear, "linear", false, false);
			Scribe_Values.Look<int>(ref anisoLevel, "anisoLevel", 0, false);
			Scribe_Values.Look<byte>(ref filterMode, "filterMode", 0, false);
			Scribe_Values.Look<byte>(ref wrapModeU, "wrapModeU", 0, false);
			Scribe_Values.Look<byte>(ref wrapModeV, "wrapModeV", 0, false);
			Scribe_Values.Look<float>(ref mipMapBias, "mipmapBias", 0f, false);
			Scribe_Values.Look<int>(ref requestedMipmapLevel, "requestedMipmapLevel", 0, false);
			Scribe_Values.Look<bool>(ref ignoreMipmapLimit, "ignoreMipmapLimit", false, false);
			Scribe_Values.Look<int>(ref minimumMipmapLevel, "minimumMipmapLevel", 0, false);
			DataExposeUtility.LookByteArray(ref data, "textureData");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.Texture = new Texture2D(width, height, (TextureFormat)textureFormat, mipCount, linear);
				this.Texture.name = name;
				this.Texture.anisoLevel = anisoLevel;
				this.Texture.filterMode = (FilterMode)filterMode;
				this.Texture.wrapModeU = (TextureWrapMode)wrapModeU;
				this.Texture.wrapModeV = (TextureWrapMode)wrapModeV;
				this.Texture.mipMapBias = mipMapBias;
				this.Texture.requestedMipmapLevel = requestedMipmapLevel;
				this.Texture.ignoreMipmapLimit = ignoreMipmapLimit;
				this.Texture.minimumMipmapLevel = minimumMipmapLevel;
				this.Texture.LoadRawTextureData(data);
				this.Texture.Apply();
			}
		}

		// Token: 0x060025A9 RID: 9641 RVA: 0x000E53E4 File Offset: 0x000E35E4
		public static explicit operator SavedTexture2D(Texture2D tex)
		{
			return new SavedTexture2D(tex);
		}

		// Token: 0x060025AA RID: 9642 RVA: 0x000E53EC File Offset: 0x000E35EC
		public static explicit operator Texture2D(SavedTexture2D tex)
		{
			return tex.Texture;
		}

		// Token: 0x060025AB RID: 9643 RVA: 0x000E53F4 File Offset: 0x000E35F4
		public override bool Equals(object obj)
		{
			SavedTexture2D savedTexture2D = obj as SavedTexture2D;
			return savedTexture2D != null && this.Texture.Equals(savedTexture2D.Texture);
		}

		// Token: 0x060025AC RID: 9644 RVA: 0x000E541E File Offset: 0x000E361E
		public override int GetHashCode()
		{
			return Gen.HashCombine<Texture2D>(74839, this.Texture);
		}

		// Token: 0x060025AD RID: 9645 RVA: 0x000E5430 File Offset: 0x000E3630
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(14, 1);
			defaultInterpolatedStringHandler.AppendLiteral("SavedTexture(");
			defaultInterpolatedStringHandler.AppendFormatted<Texture2D>(this.Texture);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x060025AE RID: 9646 RVA: 0x000E5473 File Offset: 0x000E3673
		public void Dispose()
		{
			Object.Destroy(this.Texture);
		}
	}
}
