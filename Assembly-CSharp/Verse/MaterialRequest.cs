using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000501 RID: 1281
	public struct MaterialRequest : IEquatable<MaterialRequest>
	{
		// Token: 0x170007CE RID: 1998
		// (set) Token: 0x060025FB RID: 9723 RVA: 0x000E75D3 File Offset: 0x000E57D3
		public string BaseTexPath
		{
			set
			{
				this.mainTex = ContentFinder<Texture2D>.Get(value, true);
			}
		}

		// Token: 0x060025FC RID: 9724 RVA: 0x000E75E4 File Offset: 0x000E57E4
		public MaterialRequest(Texture tex)
		{
			this.shader = ShaderDatabase.Cutout;
			this.mainTex = tex;
			this.color = Color.white;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
			this.secondaryTex = null;
			this.needsMainTex = true;
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x000E763C File Offset: 0x000E583C
		public MaterialRequest(Texture tex, Shader shader)
		{
			this.shader = shader;
			this.mainTex = tex;
			this.color = Color.white;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
			this.secondaryTex = null;
			this.needsMainTex = true;
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x000E7690 File Offset: 0x000E5890
		public MaterialRequest(Texture tex, Shader shader, Color color)
		{
			this.shader = shader;
			this.mainTex = tex;
			this.color = color;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
			this.secondaryTex = null;
			this.needsMainTex = true;
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x000E76E0 File Offset: 0x000E58E0
		public MaterialRequest(Shader shader)
		{
			this.shader = shader;
			this.mainTex = null;
			this.color = Color.white;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
			this.secondaryTex = null;
			this.needsMainTex = false;
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x000E7734 File Offset: 0x000E5934
		public override int GetHashCode()
		{
			return Gen.HashCombine<Texture>(Gen.HashCombine<List<ShaderParameter>>(Gen.HashCombineInt(Gen.HashCombine<Texture2D>(Gen.HashCombine<Texture>(Gen.HashCombineStruct<Color>(Gen.HashCombineStruct<Color>(Gen.HashCombine<Shader>(0, this.shader), this.color), this.colorTwo), this.mainTex), this.maskTex), this.renderQueue), this.shaderParameters), this.secondaryTex);
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x000E779C File Offset: 0x000E599C
		public override bool Equals(object obj)
		{
			if (obj is MaterialRequest)
			{
				MaterialRequest other = (MaterialRequest)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06002602 RID: 9730 RVA: 0x000E77C4 File Offset: 0x000E59C4
		public bool Equals(MaterialRequest other)
		{
			return other.shader == this.shader && other.mainTex == this.mainTex && other.color == this.color && other.colorTwo == this.colorTwo && other.maskTex == this.maskTex && other.renderQueue == this.renderQueue && other.secondaryTex == this.secondaryTex && other.shaderParameters == this.shaderParameters;
		}

		// Token: 0x06002603 RID: 9731 RVA: 0x000E7861 File Offset: 0x000E5A61
		public static bool operator ==(MaterialRequest lhs, MaterialRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06002604 RID: 9732 RVA: 0x000E786B File Offset: 0x000E5A6B
		public static bool operator !=(MaterialRequest lhs, MaterialRequest rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x000E7878 File Offset: 0x000E5A78
		public override string ToString()
		{
			string[] array = new string[15];
			array[0] = "MaterialRequest(";
			array[1] = this.shader.name;
			array[2] = ", ";
			array[3] = this.mainTex.name;
			array[4] = ", ";
			array[5] = this.color.ToString();
			array[6] = ", ";
			array[7] = this.colorTwo.ToString();
			array[8] = ", ";
			int num = 9;
			Texture2D texture2D = this.maskTex;
			array[num] = ((texture2D != null) ? texture2D.ToString() : null);
			array[10] = ", ";
			int num2 = 11;
			Texture texture = this.secondaryTex;
			array[num2] = ((texture != null) ? texture.ToString() : null);
			array[12] = ", ";
			array[13] = this.renderQueue.ToString();
			array[14] = ")";
			return string.Concat(array);
		}

		// Token: 0x04001B56 RID: 6998
		public Shader shader;

		// Token: 0x04001B57 RID: 6999
		public Texture mainTex;

		// Token: 0x04001B58 RID: 7000
		public Color color;

		// Token: 0x04001B59 RID: 7001
		public Color colorTwo;

		// Token: 0x04001B5A RID: 7002
		public Texture2D maskTex;

		// Token: 0x04001B5B RID: 7003
		public Texture secondaryTex;

		// Token: 0x04001B5C RID: 7004
		public int renderQueue;

		// Token: 0x04001B5D RID: 7005
		public bool needsMainTex;

		// Token: 0x04001B5E RID: 7006
		public List<ShaderParameter> shaderParameters;
	}
}
