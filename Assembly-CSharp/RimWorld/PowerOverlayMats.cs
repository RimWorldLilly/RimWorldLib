using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200136D RID: 4973
	[StaticConstructorOnStartup]
	public static class PowerOverlayMats
	{
		// Token: 0x06007B4C RID: 31564 RVA: 0x002BE258 File Offset: 0x002BC458
		static PowerOverlayMats()
		{
			Graphic graphic = GraphicDatabase.Get<Graphic_Single>("Things/Special/Power/TransmitterAtlas", PowerOverlayMats.TransmitterShader);
			PowerOverlayMats.LinkedOverlayGraphic = GraphicUtility.WrapLinked(graphic, LinkDrawerType.TransmitterOverlay);
			graphic.MatSingle.renderQueue = 3600;
			PowerOverlayMats.MatConnectorBase.renderQueue = 3600;
			PowerOverlayMats.MatConnectorLine.renderQueue = 3600;
		}

		// Token: 0x04004DD5 RID: 19925
		private const string TransmitterAtlasPath = "Things/Special/Power/TransmitterAtlas";

		// Token: 0x04004DD6 RID: 19926
		private static readonly Shader TransmitterShader = ShaderDatabase.MetaOverlay;

		// Token: 0x04004DD7 RID: 19927
		public static readonly Graphic LinkedOverlayGraphic;

		// Token: 0x04004DD8 RID: 19928
		public static readonly Material MatConnectorBase = MaterialPool.MatFrom("Things/Special/Power/OverlayBase", ShaderDatabase.MetaOverlay);

		// Token: 0x04004DD9 RID: 19929
		public static readonly Material MatConnectorLine = MaterialPool.MatFrom("Things/Special/Power/OverlayWire", ShaderDatabase.MetaOverlay);

		// Token: 0x04004DDA RID: 19930
		public static readonly Material MatConnectorAnticipated = MaterialPool.MatFrom("Things/Special/Power/OverlayWireAnticipated", ShaderDatabase.MetaOverlay);

		// Token: 0x04004DDB RID: 19931
		public static readonly Material MatConnectorBaseAnticipated = MaterialPool.MatFrom("Things/Special/Power/OverlayBaseAnticipated", ShaderDatabase.MetaOverlay);
	}
}
