using System;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022ED RID: 8941
	public class WorldFeatureTextMesh_Legacy : WorldFeatureTextMesh
	{
		// Token: 0x0600DCAE RID: 56494 RVA: 0x00522ECF File Offset: 0x005210CF
		private static void TextScaleFactor_Changed()
		{
			Find.WorldFeatures.textsCreated = false;
		}

		// Token: 0x17002664 RID: 9828
		// (get) Token: 0x0600DCAF RID: 56495 RVA: 0x00522EDC File Offset: 0x005210DC
		public override bool Active
		{
			get
			{
				return this.textMesh.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x17002665 RID: 9829
		// (get) Token: 0x0600DCB0 RID: 56496 RVA: 0x00522EEE File Offset: 0x005210EE
		public override Vector3 Position
		{
			get
			{
				return this.textMesh.transform.position;
			}
		}

		// Token: 0x17002666 RID: 9830
		// (get) Token: 0x0600DCB1 RID: 56497 RVA: 0x00522F00 File Offset: 0x00521100
		// (set) Token: 0x0600DCB2 RID: 56498 RVA: 0x00522F0D File Offset: 0x0052110D
		public override Color Color
		{
			get
			{
				return this.textMesh.color;
			}
			set
			{
				this.textMesh.color = value;
			}
		}

		// Token: 0x17002667 RID: 9831
		// (get) Token: 0x0600DCB3 RID: 56499 RVA: 0x00522F1B File Offset: 0x0052111B
		// (set) Token: 0x0600DCB4 RID: 56500 RVA: 0x00522F28 File Offset: 0x00521128
		public override string Text
		{
			get
			{
				return this.textMesh.text;
			}
			set
			{
				this.textMesh.text = value;
			}
		}

		// Token: 0x17002668 RID: 9832
		// (set) Token: 0x0600DCB5 RID: 56501 RVA: 0x00522F36 File Offset: 0x00521136
		public override float Size
		{
			set
			{
				this.textMesh.fontSize = Mathf.RoundToInt(value * WorldFeatureTextMesh_Legacy.TextScaleFactor);
			}
		}

		// Token: 0x17002669 RID: 9833
		// (get) Token: 0x0600DCB6 RID: 56502 RVA: 0x00522F4F File Offset: 0x0052114F
		// (set) Token: 0x0600DCB7 RID: 56503 RVA: 0x00522F61 File Offset: 0x00521161
		public override Quaternion Rotation
		{
			get
			{
				return this.textMesh.transform.rotation;
			}
			set
			{
				this.textMesh.transform.rotation = value;
			}
		}

		// Token: 0x1700266A RID: 9834
		// (get) Token: 0x0600DCB8 RID: 56504 RVA: 0x00522F74 File Offset: 0x00521174
		// (set) Token: 0x0600DCB9 RID: 56505 RVA: 0x00522F86 File Offset: 0x00521186
		public override Vector3 LocalPosition
		{
			get
			{
				return this.textMesh.transform.localPosition;
			}
			set
			{
				this.textMesh.transform.localPosition = value;
			}
		}

		// Token: 0x0600DCBA RID: 56506 RVA: 0x00522F99 File Offset: 0x00521199
		public override void SetActive(bool active)
		{
			this.textMesh.gameObject.SetActive(active);
		}

		// Token: 0x0600DCBB RID: 56507 RVA: 0x00522FAC File Offset: 0x005211AC
		public override void Destroy()
		{
			Object.Destroy(this.textMesh.gameObject);
		}

		// Token: 0x0600DCBC RID: 56508 RVA: 0x00522FC0 File Offset: 0x005211C0
		public override void Init()
		{
			GameObject gameObject = new GameObject("World feature name (legacy)");
			gameObject.layer = WorldCameraManager.WorldLayer;
			Object.DontDestroyOnLoad(gameObject);
			this.textMesh = gameObject.AddComponent<TextMesh>();
			this.textMesh.color = new Color(1f, 1f, 1f, 0f);
			this.textMesh.anchor = TextAnchor.MiddleCenter;
			this.textMesh.alignment = TextAlignment.Center;
			this.textMesh.GetComponent<MeshRenderer>().sharedMaterial.renderQueue = 3610;
			this.Color = new Color(1f, 1f, 1f, 0f);
			this.textMesh.transform.localScale = new Vector3(0.23f, 0.23f, 0.23f);
		}

		// Token: 0x0600DCBD RID: 56509 RVA: 0x0052308E File Offset: 0x0052128E
		public override void WrapAroundPlanetSurface(PlanetLayer layer)
		{
		}

		// Token: 0x04008D74 RID: 36212
		private TextMesh textMesh;

		// Token: 0x04008D75 RID: 36213
		private const float TextScale = 0.23f;

		// Token: 0x04008D76 RID: 36214
		private const int MinFontSize = 13;

		// Token: 0x04008D77 RID: 36215
		private const int MaxFontSize = 40;

		// Token: 0x04008D78 RID: 36216
		[TweakValue("Interface.World", 0f, 10f)]
		private static float TextScaleFactor = 7.5f;
	}
}
