using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001BA RID: 442
	public class ShaderTypeDef : Def
	{
		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000C3D RID: 3133 RVA: 0x00040A31 File Offset: 0x0003EC31
		public Shader Shader
		{
			get
			{
				if (this.shaderInt == null)
				{
					this.shaderInt = ShaderDatabase.LoadShader(this);
				}
				return this.shaderInt;
			}
		}

		// Token: 0x04000B8E RID: 2958
		[NoTranslate]
		public string shaderPath;

		// Token: 0x04000B8F RID: 2959
		[NoTranslate]
		public string uiShaderPath;

		// Token: 0x04000B90 RID: 2960
		[Unsaved(false)]
		private Shader shaderInt;
	}
}
