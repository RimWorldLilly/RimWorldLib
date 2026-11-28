using System;
using System.Collections.Generic;
using UnityEngine;
using Waylith.Rendering;

namespace Verse
{
	// Token: 0x020001B8 RID: 440
	public class ShaderDef : Def
	{
		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000C30 RID: 3120 RVA: 0x0004070C File Offset: 0x0003E90C
		public UnityEngine.Shader Shader
		{
			get
			{
				if (this.shaderInt != null)
				{
					return this.shaderInt;
				}
				Waylith.Rendering.Shader shader;
				if (ShaderRegistry.TryResolve(this.defName, out shader))
				{
					return this.shaderInt = UnityEngine.Shader.ForDef(this.defName, shader);
				}
				if (!this.warnedUnregistered)
				{
					this.warnedUnregistered = true;
					Log.Warning("Shader def '" + this.defName + "' is not registered (no engine program ported yet); using engine default.");
				}
				return UnityEngine.Shader.FromEngineShader(this.defName, null);
			}
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x00040788 File Offset: 0x0003E988
		public static implicit operator UnityEngine.Shader(ShaderDef def)
		{
			if (def == null)
			{
				return null;
			}
			return def.Shader;
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000C32 RID: 3122 RVA: 0x00040795 File Offset: 0x0003E995
		public bool IsComplex
		{
			get
			{
				return this.isComplex;
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000C33 RID: 3123 RVA: 0x0004079D File Offset: 0x0003E99D
		public bool HasComplexVersion
		{
			get
			{
				return this.complexVersion != null;
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000C34 RID: 3124 RVA: 0x000407A8 File Offset: 0x0003E9A8
		public ShaderDef ComplexVersion
		{
			get
			{
				return this.complexVersion;
			}
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x000407B0 File Offset: 0x0003E9B0
		public override IEnumerable<string> ConfigErrors()
		{
			ShaderDef.<ConfigErrors>d__16 <ConfigErrors>d__ = new ShaderDef.<ConfigErrors>d__16(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04000B87 RID: 2951
		public bool isComplex;

		// Token: 0x04000B88 RID: 2952
		[NoTranslate]
		public List<string> shaderKeywords;

		// Token: 0x04000B89 RID: 2953
		public ShaderProgramDefinition shaderProgram;

		// Token: 0x04000B8A RID: 2954
		public ShaderDef complexVersion;

		// Token: 0x04000B8B RID: 2955
		public ShaderDef iconVersion;

		// Token: 0x04000B8C RID: 2956
		[Unsaved(false)]
		private UnityEngine.Shader shaderInt;

		// Token: 0x04000B8D RID: 2957
		[Unsaved(false)]
		private bool warnedUnregistered;
	}
}
