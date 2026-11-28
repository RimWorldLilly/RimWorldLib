using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using LudeonTK;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Waylith.Assets;
using Waylith.Graphics;
using Waylith.Graphics.Textures;
using Waylith.Rendering;

namespace Verse
{
	// Token: 0x020004F9 RID: 1273
	public class StaticTextureAtlas
	{
		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x060025B4 RID: 9652 RVA: 0x000E5524 File Offset: 0x000E3724
		public UnityEngine.Texture2D ColorTexture
		{
			get
			{
				return this.colorTexture;
			}
		}

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x060025B5 RID: 9653 RVA: 0x000E552C File Offset: 0x000E372C
		public UnityEngine.Texture2D MaskTexture
		{
			get
			{
				return this.maskTexture;
			}
		}

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x060025B6 RID: 9654 RVA: 0x000E5534 File Offset: 0x000E3734
		public IEnumerable<UnityEngine.Texture2D> Members
		{
			get
			{
				return this.textures.Concat(this.masks.Values);
			}
		}

		// Token: 0x060025B7 RID: 9655 RVA: 0x000E554C File Offset: 0x000E374C
		public string CacheKey(bool mask)
		{
			if (!mask)
			{
				return this.colorCacheKey;
			}
			return this.maskCacheKey;
		}

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x060025B8 RID: 9656 RVA: 0x000E555E File Offset: 0x000E375E
		public static int MaxPixelsPerAtlas
		{
			get
			{
				return StaticTextureAtlas.MaxPagePixels / 8 * 5;
			}
		}

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x060025B9 RID: 9657 RVA: 0x000E5569 File Offset: 0x000E3769
		public static int MaxAtlasSize
		{
			get
			{
				return SystemInfo.maxTextureSize;
			}
		}

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x060025BA RID: 9658 RVA: 0x000E5570 File Offset: 0x000E3770
		public static int MaxPagePixels
		{
			get
			{
				long num = (long)StaticTextureAtlas.MaxAtlasSize * (long)StaticTextureAtlas.MaxAtlasSize / 2L;
				long deviceLocalHeapBytes = TextureResidency.DeviceLocalHeapBytes;
				if (deviceLocalHeapBytes > 0L)
				{
					num = Math.Min(num, deviceLocalHeapBytes / 6L / 16L);
				}
				num = 1L << BitOperations.Log2((ulong)num);
				return (int)Math.Max(4194304L, num);
			}
		}

		// Token: 0x060025BB RID: 9659 RVA: 0x000E55C4 File Offset: 0x000E37C4
		public StaticTextureAtlas(TextureAtlasGroupKey groupKey)
		{
			this.groupKey = groupKey;
			this.colorTexture = new UnityEngine.Texture2D(1, 1, TextureFormat.ARGB32, false);
			this.colorTexture.name = "StaticTextureAtlas";
		}

		// Token: 0x060025BC RID: 9660 RVA: 0x000E5620 File Offset: 0x000E3820
		public void Insert(UnityEngine.Texture2D texture, UnityEngine.Texture2D mask = null)
		{
			if (this.groupKey.hasMask && mask == null)
			{
				Log.Error("Tried to insert a mask-less texture into a static atlas which does have a mask atlas");
			}
			if (!this.groupKey.hasMask && mask != null)
			{
				Log.Error("Tried to insert a mask texture into a static atlas which does not have a mask atlas");
			}
			this.textures.Add(texture);
			if (mask != null && this.groupKey.hasMask)
			{
				this.masks.Add(texture, mask);
			}
		}

		// Token: 0x060025BD RID: 9661 RVA: 0x000E569C File Offset: 0x000E389C
		public void Bake(bool rebake = false)
		{
			using (new DeepProfilerScope("StaticTextureAtlas.Bake()"))
			{
				if (rebake)
				{
					foreach (KeyValuePair<UnityEngine.Texture, StaticTextureAtlasTile> keyValuePair in this.tiles)
					{
						Object.Destroy(keyValuePair.Value.mesh);
					}
					this.tiles.Clear();
				}
				Rect[] array = this.CalcRectsForAtlasNew(1);
				if (array.Length != this.textures.Count)
				{
					Log.Error("Texture packing failed! Clearing out atlas...");
					this.textures.Clear();
				}
				else
				{
					this.BuildMeshesForUvs(array);
					if (this.groupKey.hasMask)
					{
						this.CreateMaskTexture();
					}
					this.colorCacheKey = this.TryGetCacheKey(false);
					this.maskCacheKey = (this.groupKey.hasMask ? this.TryGetCacheKey(true) : null);
					this.AdoptedFromCache = this.TryAdoptCachedPages();
					if (!this.AdoptedFromCache)
					{
						this.BlitTexturesToColorAtlas(array);
						if (this.groupKey.hasMask)
						{
							this.BlitMaskAtlas(array);
						}
						GlobalTextureAtlasManager.CompressAtlas(this);
					}
				}
			}
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x060025BE RID: 9662 RVA: 0x000E57D8 File Offset: 0x000E39D8
		// (set) Token: 0x060025BF RID: 9663 RVA: 0x000E57E0 File Offset: 0x000E39E0
		public bool AdoptedFromCache { get; private set; }

		// Token: 0x060025C0 RID: 9664 RVA: 0x000E57EC File Offset: 0x000E39EC
		private bool TryAdoptCachedPages()
		{
			if (!GlobalTextureAtlasManager.AtlasCompressionEnabled)
			{
				return false;
			}
			ITextureCacheSource textureCacheSource;
			if (!this.TryOpenCachedPage(false, out textureCacheSource))
			{
				return false;
			}
			ITextureCacheSource textureCacheSource2 = null;
			if (this.groupKey.hasMask && !this.TryOpenCachedPage(true, out textureCacheSource2))
			{
				textureCacheSource.Dispose();
				return false;
			}
			using (textureCacheSource)
			{
				using (textureCacheSource2)
				{
					if (!this.colorTexture.CoreTexture.TryAdoptCompressed(textureCacheSource) || (this.groupKey.hasMask && !this.maskTexture.CoreTexture.TryAdoptCompressed(textureCacheSource2)))
					{
						this.RecreatePages();
						return false;
					}
				}
			}
			this.NameAtlasTextures();
			return true;
		}

		// Token: 0x060025C1 RID: 9665 RVA: 0x000E58B0 File Offset: 0x000E3AB0
		private void RecreatePages()
		{
			UnityEngine.Texture2D texture2D = this.colorTexture;
			this.colorTexture = this.CreatePage(texture2D.width, texture2D.height, texture2D.mipmapCount, "TextureAtlas_Working");
			Object.DestroyImmediate(texture2D);
			if (this.groupKey.hasMask)
			{
				this.CreateMaskTexture();
			}
		}

		// Token: 0x060025C2 RID: 9666 RVA: 0x000E5900 File Offset: 0x000E3B00
		private bool TryOpenCachedPage(bool mask, out ITextureCacheSource cached)
		{
			string text = this.CacheKey(mask);
			if (text != null)
			{
				return GpuTextureService.TryOpenKeyedTextureCache(text, out cached);
			}
			cached = null;
			return false;
		}

		// Token: 0x060025C3 RID: 9667 RVA: 0x000E5924 File Offset: 0x000E3B24
		private void NameAtlasTextures()
		{
			this.colorTexture.name = "TextureAtlas_" + this.groupKey.ToString() + "_" + this.colorTexture.GetInstanceID().ToString();
			if (this.maskTexture != null)
			{
				this.maskTexture.name = "Mask_" + this.colorTexture.name;
			}
		}

		// Token: 0x060025C4 RID: 9668 RVA: 0x000E59A0 File Offset: 0x000E3BA0
		private UnityEngine.Texture2D CreatePage(int width, int height, int mipCount, string name)
		{
			UnityEngine.Texture2D texture2D = new UnityEngine.Texture2D(width, height, GraphicsFormat.R8G8B8A8_SRGB, mipCount, TextureCreationFlags.DontInitializePixels | TextureCreationFlags.MipChain);
			texture2D.CoreTexture.MemoryCategory = GpuMemoryCategory.Atlas;
			texture2D.name = name;
			this.ApplyAtlasTextureSamplingSettings(texture2D);
			return texture2D;
		}

		// Token: 0x060025C5 RID: 9669 RVA: 0x000E59D4 File Offset: 0x000E3BD4
		private string TryGetCacheKey(bool mask)
		{
			UnityEngine.Texture2D texture2D = mask ? this.maskTexture : this.colorTexture;
			if (texture2D == null || this.tiles.Count != this.textures.Count)
			{
				return null;
			}
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.Write(1);
					binaryWriter.Write(typeof(StaticTextureAtlas).Assembly.ManifestModule.ModuleVersionId.ToByteArray());
					binaryWriter.Write(typeof(UnityEngine.Texture2D).Assembly.ManifestModule.ModuleVersionId.ToByteArray());
					binaryWriter.Write(typeof(GpuTextureService).Assembly.ManifestModule.ModuleVersionId.ToByteArray());
					binaryWriter.Write(TextureCompressionSettings.CacheTag);
					binaryWriter.Write(TextureCompressionSettings.Quality);
					binaryWriter.Write(this.groupKey.ToString());
					binaryWriter.Write(mask);
					binaryWriter.Write(texture2D.width);
					binaryWriter.Write(texture2D.height);
					binaryWriter.Write(texture2D.mipmapCount);
					binaryWriter.Write(8);
					foreach (UnityEngine.Texture2D texture2D2 in this.textures)
					{
						if (!StaticTextureAtlas.TryWriteSourceStamp(binaryWriter, texture2D2))
						{
							return null;
						}
						UnityEngine.Texture2D texture;
						if (mask && (!this.masks.TryGetValue(texture2D2, out texture) || !StaticTextureAtlas.TryWriteSourceStamp(binaryWriter, texture)))
						{
							return null;
						}
						StaticTextureAtlasTile staticTextureAtlasTile;
						if (!this.tiles.TryGetValue(texture2D2, out staticTextureAtlasTile))
						{
							return null;
						}
						binaryWriter.Write(staticTextureAtlasTile.uvRect.x);
						binaryWriter.Write(staticTextureAtlasTile.uvRect.y);
						binaryWriter.Write(staticTextureAtlasTile.uvRect.width);
						binaryWriter.Write(staticTextureAtlasTile.uvRect.height);
					}
					binaryWriter.Flush();
					result = Convert.ToHexString(SHA256.HashData(memoryStream.ToArray()));
				}
			}
			return result;
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x000E5C50 File Offset: 0x000E3E50
		private static bool TryWriteSourceStamp(BinaryWriter writer, UnityEngine.Texture2D texture)
		{
			string text;
			if (texture == null)
			{
				text = null;
			}
			else
			{
				Waylith.Rendering.Texture2D coreTexture = texture.CoreTexture;
				text = ((coreTexture != null) ? coreTexture.SourcePath : null);
			}
			string text2 = text;
			if (text2 == null)
			{
				return false;
			}
			writer.Write(text2);
			string value;
			if (ContentPack.TryGetTextureContentHash(text2, out value))
			{
				writer.Write(value);
				return true;
			}
			FileInfo fileInfo = new FileInfo(text2);
			if (!fileInfo.Exists)
			{
				return false;
			}
			writer.Write(fileInfo.Length);
			writer.Write(fileInfo.LastWriteTimeUtc.Ticks);
			return true;
		}

		// Token: 0x060025C7 RID: 9671 RVA: 0x000E5CC8 File Offset: 0x000E3EC8
		public void EvictSourceTextures()
		{
			foreach (UnityEngine.Texture2D texture2D in this.textures)
			{
				if (texture2D != null)
				{
					texture2D.EvictGpuResource();
				}
			}
			foreach (UnityEngine.Texture2D texture2D2 in this.masks.Values)
			{
				if (texture2D2 != null)
				{
					texture2D2.EvictGpuResource();
				}
			}
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x000E5D68 File Offset: 0x000E3F68
		private void BuildMeshesForUvs(Rect[] uvRects)
		{
			for (int i = 0; i < this.textures.Count; i++)
			{
				Mesh mesh = TextureAtlasHelper.CreateMeshForUV(uvRects[i], 0.5f);
				mesh.name = "TextureAtlasMesh_" + this.groupKey.ToString() + "_" + mesh.GetInstanceID().ToString();
				this.tiles.Add(this.textures[i], new StaticTextureAtlasTile
				{
					atlas = this,
					mesh = mesh,
					uvRect = uvRects[i]
				});
			}
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x000E5E14 File Offset: 0x000E4014
		private Rect[] CalcRectsForAtlasNew(int divisor = 1)
		{
			Rect[] result;
			using (new DeepProfilerScope("StaticTextureAtlas.CalcRectsForAtlasNew()"))
			{
				List<UnityEngine.Texture2D> list = (from t in this.textures
				orderby t.height descending
				select t).ToList<UnityEngine.Texture2D>();
				int num = Mathf.NextPowerOfTwo((int)((float)Mathf.CeilToInt(Mathf.Sqrt((float)list.Sum((UnityEngine.Texture2D t) => (t.width + 8) * (t.height + 8)))) * 1.05f));
				num = Mathf.Min(num, StaticTextureAtlas.MaxAtlasSize);
				List<Vector2> list2 = (from t in list
				select new Vector2((float)Mathf.RoundToInt((float)t.width / (float)divisor), (float)Mathf.RoundToInt((float)t.height / (float)divisor))).ToList<Vector2>();
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				Rect[] array = new Rect[list.Count];
				for (int i = 0; i < list.Count; i++)
				{
					Vector2 vector = list2[i];
					if ((float)num2 + vector.x > (float)num)
					{
						num2 = 0;
						num3 += num4 + 8;
						num4 = 0;
					}
					array[i] = new Rect((float)num2, (float)num3, vector.x, vector.y);
					num2 += Mathf.CeilToInt(vector.x) + 8;
					if (vector.y > (float)num4)
					{
						num4 = Mathf.CeilToInt(vector.y);
					}
				}
				int num5 = num3 + num4;
				int num6 = Mathf.NextPowerOfTwo(num5);
				if (num5 > StaticTextureAtlas.MaxAtlasSize || (long)num * (long)num6 > (long)StaticTextureAtlas.MaxPagePixels)
				{
					if (divisor < 4)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(110, 5);
						defaultInterpolatedStringHandler.AppendLiteral("StaticTextureAtlas: Textures pack to ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(num);
						defaultInterpolatedStringHandler.AppendLiteral("x");
						defaultInterpolatedStringHandler.AppendFormatted<int>(num5);
						defaultInterpolatedStringHandler.AppendLiteral(", over the page limit of ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(StaticTextureAtlas.MaxPagePixels);
						defaultInterpolatedStringHandler.AppendLiteral(" pixels (max axis ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(StaticTextureAtlas.MaxAtlasSize);
						defaultInterpolatedStringHandler.AppendLiteral("). Retrying with divisor of ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(divisor * 2);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
						result = this.CalcRectsForAtlasNew(divisor * 2);
					}
					else
					{
						result = this.CalcRectsForAtlas();
					}
				}
				else
				{
					Rect[] array2 = new Rect[list.Count];
					for (int j = 0; j < this.textures.Count; j++)
					{
						UnityEngine.Texture2D texture2D = this.textures[j];
						int num7 = list.IndexOf(texture2D);
						if (num7 < 0)
						{
							Log.Error("Texture " + texture2D.name + " not found in height sorted list, cannot calculate UV rect.");
						}
						else
						{
							Rect rect = array[num7];
							array2[j] = new Rect(rect.x / (float)num, rect.y / (float)num6, rect.width / (float)num, rect.height / (float)num6);
						}
					}
					Object obj = this.colorTexture;
					int mipCount = Mathf.FloorToInt(Mathf.Log((float)Mathf.Min(num, num6) / 512f, 2f)) + 1;
					this.colorTexture = this.CreatePage(num, num6, mipCount, "TextureAtlas_Working");
					Object.DestroyImmediate(obj);
					result = array2;
				}
			}
			return result;
		}

		// Token: 0x060025CA RID: 9674 RVA: 0x000E617C File Offset: 0x000E437C
		private Rect[] CalcRectsForAtlas()
		{
			UnityEngine.Texture2D[] array;
			using (new DeepProfilerScope("Create dummy textures for atlas packing"))
			{
				array = this.textures.Select(delegate(UnityEngine.Texture2D t)
				{
					int width = t.width;
					int height = t.height;
					bool flag = width % 4 == 0 && height % 4 == 0;
					return new UnityEngine.Texture2D(width, height, flag ? TextureFormat.DXT1 : TextureFormat.Alpha8, 1, false, true)
					{
						name = t.name + "_DummyForAtlas",
						filterMode = t.filterMode,
						wrapMode = t.wrapMode,
						anisoLevel = t.anisoLevel,
						mipMapBias = t.mipMapBias,
						minimumMipmapLevel = t.minimumMipmapLevel
					};
				}).ToArray<UnityEngine.Texture2D>();
			}
			Rect[] result;
			using (new DeepProfilerScope("PackTextures() with dummy textures"))
			{
				result = this.colorTexture.PackTextures(array, 8, StaticTextureAtlas.MaxAtlasSize, false);
				UnityEngine.Texture2D texture2D = this.colorTexture;
				UnityEngine.Texture2D[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					Object.DestroyImmediate(array2[i]);
				}
				int mipCount = Mathf.FloorToInt(Mathf.Log((float)Mathf.Min(texture2D.width, texture2D.height) / 512f, 2f)) + 1;
				this.colorTexture = this.CreatePage(texture2D.width, texture2D.height, mipCount, "TextureAtlas_Working");
				Object.DestroyImmediate(texture2D);
			}
			return result;
		}

		// Token: 0x060025CB RID: 9675 RVA: 0x000E6290 File Offset: 0x000E4490
		private void BlitTexturesToColorAtlas(Rect[] uvRects)
		{
			UnityEngine.RenderTexture renderTexture = new UnityEngine.RenderTexture(this.colorTexture.width, this.colorTexture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear)
			{
				name = "StaticTextureAtlas_Bake_Temp"
			};
			renderTexture.Create();
			UnityEngine.RenderTexture active = UnityEngine.RenderTexture.active;
			UnityEngine.Graphics.SetRenderTarget(renderTexture);
			GL.Clear(true, true, Color.clear);
			GL.PushMatrix();
			GL.LoadPixelMatrix(0f, (float)renderTexture.width, (float)renderTexture.height, 0f);
			using (new DeepProfilerScope("Render real textures to RT"))
			{
				UnityEngine.Material material = MaterialAllocator.Create(UnityEngine.Shader.Find("Custom/BlitExact"));
				material.name = "StaticTextureAtlas.BlitTexturesToColorAtlas.Blit";
				try
				{
					for (int i = 0; i < this.textures.Count; i++)
					{
						Rect rect = uvRects[i];
						UnityEngine.Texture2D texture = this.textures[i];
						float num = (float)Mathf.RoundToInt(rect.x * (float)this.colorTexture.width);
						int num2 = Mathf.RoundToInt((1f - rect.y) * (float)this.colorTexture.height);
						int num3 = Mathf.RoundToInt(rect.width * (float)this.colorTexture.width);
						int num4 = Mathf.RoundToInt(rect.height * (float)this.colorTexture.height);
						num2 -= num4;
						UnityEngine.Graphics.DrawTexture(new Rect(num, (float)num2, (float)num3, (float)num4), texture, new Rect(0f, 0f, 1f, 1f), 0, 0, 0, 0, material, -1);
					}
				}
				finally
				{
					MaterialAllocator.Destroy(material);
				}
			}
			GL.PopMatrix();
			using (new DeepProfilerScope("CopyTexture from RenderTexture to Texture2D"))
			{
				UnityEngine.Graphics.CopyTexture(renderTexture, 0, 0, 0, 0, this.colorTexture.width, this.colorTexture.height, this.colorTexture, 0, 0, 0, 0);
			}
			UnityEngine.RenderTexture.active = active;
			using (new DeepProfilerScope("Generate mipmaps"))
			{
				GenTexture.GenerateMipmaps(this.colorTexture, 512, false, 0.25f);
			}
			renderTexture.Release();
			Object.DestroyImmediate(renderTexture);
			this.colorTexture.name = "TextureAtlas_" + this.groupKey.ToString() + "_" + this.colorTexture.GetInstanceID().ToString();
		}

		// Token: 0x060025CC RID: 9676 RVA: 0x000E6554 File Offset: 0x000E4754
		private void CreateMaskTexture()
		{
			if (this.maskTexture != null)
			{
				Object.DestroyImmediate(this.maskTexture);
			}
			this.maskTexture = this.CreatePage(this.colorTexture.width, this.colorTexture.height, this.colorTexture.mipmapCount, "TextureAtlasMask_Working");
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x000E65AC File Offset: 0x000E47AC
		private void BlitMaskAtlas(Rect[] uvRects)
		{
			UnityEngine.RenderTexture renderTexture = new UnityEngine.RenderTexture(this.maskTexture.width, this.maskTexture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear)
			{
				name = "StaticTextureAtlas_MaskBake_Temp"
			};
			renderTexture.Create();
			UnityEngine.RenderTexture active = UnityEngine.RenderTexture.active;
			UnityEngine.Graphics.SetRenderTarget(renderTexture);
			GL.Clear(true, true, Color.clear);
			GL.PushMatrix();
			GL.LoadPixelMatrix(0f, (float)renderTexture.width, (float)renderTexture.height, 0f);
			using (new DeepProfilerScope("Render mask textures to RT"))
			{
				UnityEngine.Material material = MaterialAllocator.Create(UnityEngine.Shader.Find("Custom/BlitExact"));
				material.name = "StaticTextureAtlas.BuildMaskAtlas.Blit";
				try
				{
					for (int i = 0; i < this.textures.Count; i++)
					{
						UnityEngine.Texture2D key = this.textures[i];
						UnityEngine.Texture2D texture;
						if (this.masks.TryGetValue(key, out texture))
						{
							Rect rect = uvRects[i];
							float num = (float)Mathf.RoundToInt(rect.x * (float)this.maskTexture.width);
							int num2 = Mathf.RoundToInt((1f - rect.y) * (float)this.maskTexture.height);
							int num3 = Mathf.RoundToInt(rect.width * (float)this.maskTexture.width);
							int num4 = Mathf.RoundToInt(rect.height * (float)this.maskTexture.height);
							num2 -= num4;
							UnityEngine.Graphics.DrawTexture(new Rect(num, (float)num2, (float)num3, (float)num4), texture, new Rect(0f, 0f, 1f, 1f), 0, 0, 0, 0, material, -1);
						}
					}
				}
				finally
				{
					MaterialAllocator.Destroy(material);
				}
			}
			GL.PopMatrix();
			using (new DeepProfilerScope("CopyTexture from RenderTexture to mask Texture2D"))
			{
				UnityEngine.Graphics.CopyTexture(renderTexture, 0, 0, 0, 0, this.maskTexture.width, this.maskTexture.height, this.maskTexture, 0, 0, 0, 0);
			}
			UnityEngine.RenderTexture.active = active;
			using (new DeepProfilerScope("Generate mipmaps for mask texture"))
			{
				GenTexture.GenerateMipmaps(this.maskTexture, 512, false, 0.5f);
			}
			renderTexture.Release();
			Object.DestroyImmediate(renderTexture);
			this.maskTexture.name = "Mask_" + this.colorTexture.name;
		}

		// Token: 0x060025CE RID: 9678 RVA: 0x000E6860 File Offset: 0x000E4A60
		public bool TryGetTile(UnityEngine.Texture texture, out StaticTextureAtlasTile tile)
		{
			return this.tiles.TryGetValue(texture, out tile);
		}

		// Token: 0x060025CF RID: 9679 RVA: 0x000E6870 File Offset: 0x000E4A70
		private void ApplyAtlasTextureSamplingSettings(UnityEngine.Texture2D texture)
		{
			if (texture == null)
			{
				return;
			}
			UnityEngine.FilterMode filterMode = UnityEngine.FilterMode.Bilinear;
			int num = 1;
			float num2 = 0f;
			for (int i = 0; i < this.textures.Count; i++)
			{
				UnityEngine.Texture2D texture2D = this.textures[i];
				if (!(texture2D == null))
				{
					if (texture2D.filterMode == UnityEngine.FilterMode.Trilinear)
					{
						filterMode = UnityEngine.FilterMode.Trilinear;
					}
					else if (texture2D.filterMode == UnityEngine.FilterMode.Point && filterMode == UnityEngine.FilterMode.Bilinear)
					{
						filterMode = UnityEngine.FilterMode.Point;
					}
					num = Math.Max(num, texture2D.anisoLevel);
					num2 = Math.Min(num2, texture2D.mipMapBias);
				}
			}
			texture.filterMode = filterMode;
			texture.wrapMode = UnityEngine.TextureWrapMode.Clamp;
			texture.anisoLevel = Math.Max(2, num);
			texture.mipMapBias = num2;
			texture.minimumMipmapLevel = 0;
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x000E6921 File Offset: 0x000E4B21
		public static UnityEngine.Texture2D FastCompressDXT(UnityEngine.Texture2D texture, bool deleteOriginal = false)
		{
			if (Mathf.Min(texture.width, texture.height) <= 16)
			{
				return texture;
			}
			texture.Compress(true);
			return texture;
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x000E6944 File Offset: 0x000E4B44
		public void Destroy()
		{
			Object.Destroy(this.colorTexture);
			Object.Destroy(this.maskTexture);
			foreach (KeyValuePair<UnityEngine.Texture, StaticTextureAtlasTile> keyValuePair in this.tiles)
			{
				Object.Destroy(keyValuePair.Value.mesh);
			}
			this.textures.Clear();
			this.masks.Clear();
			this.tiles.Clear();
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x000E69D8 File Offset: 0x000E4BD8
		public static int CalculateMaxMipmapsForDxtSupport(UnityEngine.Texture2D tex)
		{
			int num = 0;
			int num2 = tex.width;
			int num3 = tex.height;
			while (num2 >= 4 && num3 >= 4 && num2 % 4 == 0 && num3 % 4 == 0)
			{
				num++;
				num2 >>= 1;
				num3 >>= 1;
			}
			if (num == 0)
			{
				num = 1;
			}
			return num;
		}

		// Token: 0x04001B38 RID: 6968
		public readonly TextureAtlasGroupKey groupKey;

		// Token: 0x04001B39 RID: 6969
		private List<UnityEngine.Texture2D> textures = new List<UnityEngine.Texture2D>();

		// Token: 0x04001B3A RID: 6970
		private Dictionary<UnityEngine.Texture2D, UnityEngine.Texture2D> masks = new Dictionary<UnityEngine.Texture2D, UnityEngine.Texture2D>();

		// Token: 0x04001B3B RID: 6971
		private Dictionary<UnityEngine.Texture, StaticTextureAtlasTile> tiles = new Dictionary<UnityEngine.Texture, StaticTextureAtlasTile>();

		// Token: 0x04001B3C RID: 6972
		private UnityEngine.Texture2D colorTexture;

		// Token: 0x04001B3D RID: 6973
		private UnityEngine.Texture2D maskTexture;

		// Token: 0x04001B3E RID: 6974
		private string colorCacheKey;

		// Token: 0x04001B3F RID: 6975
		private string maskCacheKey;

		// Token: 0x04001B40 RID: 6976
		public const int MaxTextureSizeForTiles = 512;

		// Token: 0x04001B41 RID: 6977
		public const int TexturePadding = 8;

		// Token: 0x04001B42 RID: 6978
		private const long BakeBytesPerPagePixel = 16L;

		// Token: 0x04001B43 RID: 6979
		private const int MinPagePixels = 4194304;

		// Token: 0x04001B45 RID: 6981
		private const int CacheBakeVersion = 1;
	}
}
