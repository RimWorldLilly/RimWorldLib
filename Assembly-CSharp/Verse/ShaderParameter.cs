using System;
using System.Xml;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000AB RID: 171
	public class ShaderParameter
	{
		// Token: 0x0600073D RID: 1853 RVA: 0x00029B38 File Offset: 0x00027D38
		public void Apply(Material mat)
		{
			switch (this.type)
			{
			case ShaderParameter.Type.Float:
				mat.SetFloat(this.name, this.value.x);
				return;
			case ShaderParameter.Type.Vector:
				mat.SetVector(this.name, this.value);
				return;
			case ShaderParameter.Type.Matrix:
				break;
			case ShaderParameter.Type.Texture:
				if (this.valueTex == null)
				{
					Log.ErrorOnce("Texture for " + this.name + " is not yet loaded; file may be invalid, or main thread may not have loaded it yet", 27929440);
				}
				mat.SetTexture(this.name, this.valueTex);
				break;
			default:
				return;
			}
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x00029BD0 File Offset: 0x00027DD0
		public void Apply(MaterialPropertyBlock block)
		{
			switch (this.type)
			{
			case ShaderParameter.Type.Float:
				block.SetFloat(this.name, this.value.x);
				return;
			case ShaderParameter.Type.Vector:
				block.SetVector(this.name, this.value);
				return;
			case ShaderParameter.Type.Matrix:
				break;
			case ShaderParameter.Type.Texture:
				if (this.valueTex == null)
				{
					Log.ErrorOnce("Texture for " + this.name + " is not yet loaded; file may be invalid, or main thread may not have loaded it yet", 27929440);
				}
				block.SetTexture(this.name, this.valueTex);
				break;
			default:
				return;
			}
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x00029C68 File Offset: 0x00027E68
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured ShaderParameter: " + xmlRoot.OuterXml);
				return;
			}
			this.name = xmlRoot.Name;
			string valstr = xmlRoot.FirstChild.Value;
			if (!valstr.NullOrEmpty() && valstr[0] == '(')
			{
				this.value = ParseHelper.FromStringVector4Adaptive(valstr);
				this.type = ShaderParameter.Type.Vector;
				return;
			}
			if (!valstr.NullOrEmpty() && valstr[0] == '/')
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.valueTex = ContentFinder<Texture2D>.Get(valstr.TrimStart('/'), true);
				});
				this.type = ShaderParameter.Type.Texture;
				return;
			}
			this.value = Vector4.one * ParseHelper.FromString<float>(valstr);
			this.type = ShaderParameter.Type.Float;
		}

		// Token: 0x040002B6 RID: 694
		[NoTranslate]
		private string name;

		// Token: 0x040002B7 RID: 695
		private Vector4 value;

		// Token: 0x040002B8 RID: 696
		private Texture2D valueTex;

		// Token: 0x040002B9 RID: 697
		private ShaderParameter.Type type;

		// Token: 0x020024DF RID: 9439
		private enum Type
		{
			// Token: 0x040096DC RID: 38620
			Float,
			// Token: 0x040096DD RID: 38621
			Vector,
			// Token: 0x040096DE RID: 38622
			Matrix,
			// Token: 0x040096DF RID: 38623
			Texture
		}
	}
}
