using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000187 RID: 391
	public class MaterialDef : Def
	{
		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000B34 RID: 2868 RVA: 0x0003CE30 File Offset: 0x0003B030
		public Material Material
		{
			get
			{
				Material result;
				if ((result = this.materialInt) == null)
				{
					result = (this.materialInt = this.CreateMaterial());
				}
				return result;
			}
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x0003CE58 File Offset: 0x0003B058
		public Material CreateMaterialInstance(int renderQueueOverride = -1)
		{
			Material material = MaterialAllocator.Create(this.Material);
			material.name = this.defName;
			if (renderQueueOverride != -1)
			{
				material.renderQueue = renderQueueOverride;
			}
			return material;
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x0003CE8C File Offset: 0x0003B08C
		public Material CreateMaterial()
		{
			Material material = MaterialAllocator.Create(this.ResolveShader());
			material.name = this.defName;
			if (this.renderQueue != -1)
			{
				material.renderQueue = this.renderQueue;
			}
			this.ApplyTextures(material);
			this.ApplyFloats(material);
			this.ApplyColors(material);
			this.ApplyVectors(material);
			this.ApplyKeywords(material);
			return material;
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x0003CEEA File Offset: 0x0003B0EA
		public override IEnumerable<string> ConfigErrors()
		{
			MaterialDef.<ConfigErrors>d__13 <ConfigErrors>d__ = new MaterialDef.<ConfigErrors>d__13(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x0003CEFA File Offset: 0x0003B0FA
		private Shader ResolveShader()
		{
			if (this.shader != null)
			{
				return this.shader.Shader;
			}
			return ShaderDatabase.DefaultShader;
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x0003CF18 File Offset: 0x0003B118
		private void ApplyTextures(Material material)
		{
			if (this.textures.NullOrEmpty<MaterialTextureProperty>())
			{
				return;
			}
			for (int i = 0; i < this.textures.Count; i++)
			{
				MaterialTextureProperty materialTextureProperty = this.textures[i];
				if (materialTextureProperty != null && !materialTextureProperty.name.NullOrEmpty() && !materialTextureProperty.path.NullOrEmpty())
				{
					int nameID = Shader.PropertyToID(materialTextureProperty.name);
					Texture2D texture2D = ContentFinder<Texture2D>.Get(materialTextureProperty.path, false);
					if (texture2D != null && materialTextureProperty.alphaFromGrayscale)
					{
						texture2D = MaterialDefUtility.AlphaFromGrayscale(materialTextureProperty.path, texture2D);
					}
					if (texture2D != null)
					{
						material.SetTexture(nameID, texture2D);
					}
					else
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 4);
						defaultInterpolatedStringHandler.AppendFormatted("MaterialDef");
						defaultInterpolatedStringHandler.AppendLiteral(" '");
						defaultInterpolatedStringHandler.AppendFormatted(this.defName);
						defaultInterpolatedStringHandler.AppendLiteral("': could not load texture '");
						defaultInterpolatedStringHandler.AppendFormatted(materialTextureProperty.path);
						defaultInterpolatedStringHandler.AppendLiteral("' for property '");
						defaultInterpolatedStringHandler.AppendFormatted(materialTextureProperty.name);
						defaultInterpolatedStringHandler.AppendLiteral("'.");
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					material.SetTextureOffset(nameID, materialTextureProperty.offset);
					material.SetTextureScale(nameID, (materialTextureProperty.scale == default(Vector2)) ? Vector2.one : materialTextureProperty.scale);
				}
			}
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x0003D080 File Offset: 0x0003B280
		private void ApplyFloats(Material material)
		{
			if (this.floats.NullOrEmpty<MaterialFloatProperty>())
			{
				return;
			}
			for (int i = 0; i < this.floats.Count; i++)
			{
				MaterialFloatProperty materialFloatProperty = this.floats[i];
				if (materialFloatProperty != null && !materialFloatProperty.name.NullOrEmpty())
				{
					material.SetFloat(materialFloatProperty.name, materialFloatProperty.value);
				}
			}
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x0003D0E0 File Offset: 0x0003B2E0
		private void ApplyColors(Material material)
		{
			if (this.colors.NullOrEmpty<MaterialColorProperty>())
			{
				return;
			}
			for (int i = 0; i < this.colors.Count; i++)
			{
				MaterialColorProperty materialColorProperty = this.colors[i];
				if (materialColorProperty != null && !materialColorProperty.name.NullOrEmpty())
				{
					material.SetColor(materialColorProperty.name, materialColorProperty.value);
				}
			}
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x0003D140 File Offset: 0x0003B340
		private void ApplyVectors(Material material)
		{
			if (this.vectors.NullOrEmpty<MaterialVectorProperty>())
			{
				return;
			}
			for (int i = 0; i < this.vectors.Count; i++)
			{
				MaterialVectorProperty materialVectorProperty = this.vectors[i];
				if (materialVectorProperty != null && !materialVectorProperty.name.NullOrEmpty())
				{
					material.SetVector(materialVectorProperty.name, materialVectorProperty.value);
				}
			}
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x0003D1A0 File Offset: 0x0003B3A0
		private void ApplyKeywords(Material material)
		{
			if (this.keywords.NullOrEmpty<string>())
			{
				return;
			}
			for (int i = 0; i < this.keywords.Count; i++)
			{
				material.EnableKeyword(this.keywords[i]);
			}
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0003D1E3 File Offset: 0x0003B3E3
		private IEnumerable<string> ValidateNamedProperties<T>(List<T> properties, string listName) where T : MaterialNamedProperty
		{
			MaterialDef.<ValidateNamedProperties>d__20<T> <ValidateNamedProperties>d__ = new MaterialDef.<ValidateNamedProperties>d__20<T>(-2);
			<ValidateNamedProperties>d__.<>3__properties = properties;
			<ValidateNamedProperties>d__.<>3__listName = listName;
			return <ValidateNamedProperties>d__;
		}

		// Token: 0x04000988 RID: 2440
		[NoTranslate]
		public string resourcePath;

		// Token: 0x04000989 RID: 2441
		public ShaderDef shader;

		// Token: 0x0400098A RID: 2442
		public int renderQueue = -1;

		// Token: 0x0400098B RID: 2443
		public List<MaterialTextureProperty> textures;

		// Token: 0x0400098C RID: 2444
		public List<MaterialFloatProperty> floats;

		// Token: 0x0400098D RID: 2445
		public List<MaterialColorProperty> colors;

		// Token: 0x0400098E RID: 2446
		public List<MaterialVectorProperty> vectors;

		// Token: 0x0400098F RID: 2447
		[NoTranslate]
		public List<string> keywords;

		// Token: 0x04000990 RID: 2448
		[Unsaved(false)]
		private Material materialInt;
	}
}
