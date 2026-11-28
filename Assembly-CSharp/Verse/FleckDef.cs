using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200025F RID: 607
	public class FleckDef : Def
	{
		// Token: 0x17000359 RID: 857
		// (get) Token: 0x060011B4 RID: 4532 RVA: 0x00065D03 File Offset: 0x00063F03
		public float Lifetime
		{
			get
			{
				return this.fadeInTime + this.solidTime + this.fadeOutTime;
			}
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x00065D19 File Offset: 0x00063F19
		public override IEnumerable<string> ConfigErrors()
		{
			FleckDef.<ConfigErrors>d__29 <ConfigErrors>d__ = new FleckDef.<ConfigErrors>d__29(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x00065D2C File Offset: 0x00063F2C
		public GraphicData GetGraphicData(int id)
		{
			if (this.graphicData != null)
			{
				return this.graphicData;
			}
			Rand.PushState(id);
			GraphicData result;
			try
			{
				result = this.randomGraphics.RandomElement<GraphicData>();
			}
			finally
			{
				Rand.PopState();
			}
			return result;
		}

		// Token: 0x04001001 RID: 4097
		public Type fleckSystemClass;

		// Token: 0x04001002 RID: 4098
		public AltitudeLayer altitudeLayer;

		// Token: 0x04001003 RID: 4099
		public float altitudeLayerIncOffset;

		// Token: 0x04001004 RID: 4100
		public bool drawGUIOverlay;

		// Token: 0x04001005 RID: 4101
		public GraphicData graphicData;

		// Token: 0x04001006 RID: 4102
		public List<GraphicData> randomGraphics;

		// Token: 0x04001007 RID: 4103
		public bool realTime;

		// Token: 0x04001008 RID: 4104
		public bool attachedToHead;

		// Token: 0x04001009 RID: 4105
		public bool useAttachLink;

		// Token: 0x0400100A RID: 4106
		public float fadeInTime;

		// Token: 0x0400100B RID: 4107
		public float solidTime = 1f;

		// Token: 0x0400100C RID: 4108
		public float fadeOutTime;

		// Token: 0x0400100D RID: 4109
		public Vector3 acceleration = Vector3.zero;

		// Token: 0x0400100E RID: 4110
		public FloatRange speedPerTime = FloatRange.Zero;

		// Token: 0x0400100F RID: 4111
		public float growthRate;

		// Token: 0x04001010 RID: 4112
		public List<CurvedScaler> scalers;

		// Token: 0x04001011 RID: 4113
		public bool collide;

		// Token: 0x04001012 RID: 4114
		public SoundDef landSound;

		// Token: 0x04001013 RID: 4115
		public Vector3 unattachedDrawOffset;

		// Token: 0x04001014 RID: 4116
		public Vector3 attachedDrawOffset;

		// Token: 0x04001015 RID: 4117
		public bool rotateTowardsMoveDirection;

		// Token: 0x04001016 RID: 4118
		public float rotateTowardsMoveDirectionExtraAngle;

		// Token: 0x04001017 RID: 4119
		public bool drawOffscreen;

		// Token: 0x04001018 RID: 4120
		public FloatRange archHeight = FloatRange.Zero;

		// Token: 0x04001019 RID: 4121
		public FloatRange archDuration;

		// Token: 0x0400101A RID: 4122
		public SimpleCurve archCurve;

		// Token: 0x0400101B RID: 4123
		public Vector3 scalingAnchor = Vector3.one * 0.5f;
	}
}
