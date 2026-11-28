using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000556 RID: 1366
	public struct GraphicRequest : IEquatable<GraphicRequest>
	{
		// Token: 0x060028AF RID: 10415 RVA: 0x000FAB00 File Offset: 0x000F8D00
		public GraphicRequest(Type graphicClass, string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo, GraphicData graphicData, int renderQueue, List<ShaderParameter> shaderParameters, string maskPath)
		{
			this.graphicClass = graphicClass;
			this.path = path;
			this.maskPath = maskPath;
			this.shader = shader;
			this.drawSize = drawSize;
			this.color = color;
			this.colorTwo = colorTwo;
			this.graphicData = graphicData;
			this.renderQueue = renderQueue;
			this.shaderParameters = (shaderParameters.NullOrEmpty<ShaderParameter>() ? null : shaderParameters);
			this.texture = null;
		}

		// Token: 0x060028B0 RID: 10416 RVA: 0x000FAB70 File Offset: 0x000F8D70
		public GraphicRequest(Type graphicClass, Texture2D texture, Shader shader, Vector2 drawSize, Color color, Color colorTwo, GraphicData graphicData, int renderQueue, List<ShaderParameter> shaderParameters, string maskPath)
		{
			this.graphicClass = graphicClass;
			this.texture = texture;
			this.maskPath = maskPath;
			this.shader = shader;
			this.drawSize = drawSize;
			this.color = color;
			this.colorTwo = colorTwo;
			this.graphicData = graphicData;
			this.renderQueue = renderQueue;
			this.shaderParameters = (shaderParameters.NullOrEmpty<ShaderParameter>() ? null : shaderParameters);
			this.path = null;
		}

		// Token: 0x060028B1 RID: 10417 RVA: 0x000FABE0 File Offset: 0x000F8DE0
		public override int GetHashCode()
		{
			if (this.path == null)
			{
				this.path = BaseContent.BadTexPath;
			}
			return Gen.HashCombine<List<ShaderParameter>>(Gen.HashCombine<int>(Gen.HashCombine<GraphicData>(Gen.HashCombineStruct<Color>(Gen.HashCombineStruct<Color>(Gen.HashCombineStruct<Vector2>(Gen.HashCombine<Shader>(Gen.HashCombine<string>(Gen.HashCombine<Texture2D>(Gen.HashCombine<string>(Gen.HashCombine<Type>(0, this.graphicClass), this.path), this.texture), this.maskPath), this.shader), this.drawSize), this.color), this.colorTwo), this.graphicData), this.renderQueue), this.shaderParameters);
		}

		// Token: 0x060028B2 RID: 10418 RVA: 0x000FAC7C File Offset: 0x000F8E7C
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted<Type>(this.graphicClass);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted(this.path);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x060028B3 RID: 10419 RVA: 0x000FACBF File Offset: 0x000F8EBF
		public override bool Equals(object obj)
		{
			return obj is GraphicRequest && this.Equals((GraphicRequest)obj);
		}

		// Token: 0x060028B4 RID: 10420 RVA: 0x000FACD8 File Offset: 0x000F8ED8
		public bool Equals(GraphicRequest other)
		{
			return this.graphicClass == other.graphicClass && this.path == other.path && this.texture == other.texture && this.maskPath == other.maskPath && this.shader == other.shader && this.drawSize == other.drawSize && this.color == other.color && this.colorTwo == other.colorTwo && this.graphicData == other.graphicData && this.renderQueue == other.renderQueue && this.shaderParameters == other.shaderParameters;
		}

		// Token: 0x060028B5 RID: 10421 RVA: 0x000FADB2 File Offset: 0x000F8FB2
		public static bool operator ==(GraphicRequest lhs, GraphicRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060028B6 RID: 10422 RVA: 0x000FADBC File Offset: 0x000F8FBC
		public static bool operator !=(GraphicRequest lhs, GraphicRequest rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x04001CA7 RID: 7335
		public Type graphicClass;

		// Token: 0x04001CA8 RID: 7336
		public Texture2D texture;

		// Token: 0x04001CA9 RID: 7337
		public string path;

		// Token: 0x04001CAA RID: 7338
		public string maskPath;

		// Token: 0x04001CAB RID: 7339
		public Shader shader;

		// Token: 0x04001CAC RID: 7340
		public Vector2 drawSize;

		// Token: 0x04001CAD RID: 7341
		public Color color;

		// Token: 0x04001CAE RID: 7342
		public Color colorTwo;

		// Token: 0x04001CAF RID: 7343
		public GraphicData graphicData;

		// Token: 0x04001CB0 RID: 7344
		public int renderQueue;

		// Token: 0x04001CB1 RID: 7345
		public List<ShaderParameter> shaderParameters;
	}
}
