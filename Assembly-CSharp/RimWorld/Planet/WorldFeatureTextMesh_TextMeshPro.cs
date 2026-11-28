using System;
using LudeonTK;
using TMPro;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022EE RID: 8942
	[StaticConstructorOnStartup]
	public class WorldFeatureTextMesh_TextMeshPro : WorldFeatureTextMesh
	{
		// Token: 0x0600DCC0 RID: 56512 RVA: 0x005230A4 File Offset: 0x005212A4
		private static void TextScale_Changed()
		{
			Find.WorldFeatures.textsCreated = false;
		}

		// Token: 0x1700266B RID: 9835
		// (get) Token: 0x0600DCC1 RID: 56513 RVA: 0x005230B1 File Offset: 0x005212B1
		public override bool Active
		{
			get
			{
				return this.textMesh.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x1700266C RID: 9836
		// (get) Token: 0x0600DCC2 RID: 56514 RVA: 0x005230C3 File Offset: 0x005212C3
		public override Vector3 Position
		{
			get
			{
				return this.textMesh.transform.position;
			}
		}

		// Token: 0x1700266D RID: 9837
		// (get) Token: 0x0600DCC3 RID: 56515 RVA: 0x005230D5 File Offset: 0x005212D5
		// (set) Token: 0x0600DCC4 RID: 56516 RVA: 0x005230E2 File Offset: 0x005212E2
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

		// Token: 0x1700266E RID: 9838
		// (get) Token: 0x0600DCC5 RID: 56517 RVA: 0x005230F0 File Offset: 0x005212F0
		// (set) Token: 0x0600DCC6 RID: 56518 RVA: 0x005230FD File Offset: 0x005212FD
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

		// Token: 0x1700266F RID: 9839
		// (set) Token: 0x0600DCC7 RID: 56519 RVA: 0x0052310B File Offset: 0x0052130B
		public override float Size
		{
			set
			{
				this.textMesh.fontSize = value * WorldFeatureTextMesh_TextMeshPro.TextScale;
			}
		}

		// Token: 0x17002670 RID: 9840
		// (get) Token: 0x0600DCC8 RID: 56520 RVA: 0x0052311F File Offset: 0x0052131F
		// (set) Token: 0x0600DCC9 RID: 56521 RVA: 0x00523131 File Offset: 0x00521331
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

		// Token: 0x17002671 RID: 9841
		// (get) Token: 0x0600DCCA RID: 56522 RVA: 0x00523144 File Offset: 0x00521344
		// (set) Token: 0x0600DCCB RID: 56523 RVA: 0x00523156 File Offset: 0x00521356
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

		// Token: 0x0600DCCC RID: 56524 RVA: 0x00523169 File Offset: 0x00521369
		public override void SetActive(bool active)
		{
			this.textMesh.gameObject.SetActive(active);
		}

		// Token: 0x0600DCCD RID: 56525 RVA: 0x0052317C File Offset: 0x0052137C
		public override void Destroy()
		{
			Object.Destroy(this.textMesh.gameObject);
		}

		// Token: 0x0600DCCE RID: 56526 RVA: 0x00523190 File Offset: 0x00521390
		public override void Init()
		{
			GameObject gameObject = Object.Instantiate<GameObject>(WorldFeatureTextMesh_TextMeshPro.WorldTextPrefab);
			Object.DontDestroyOnLoad(gameObject);
			this.textMesh = gameObject.GetComponent<TextMeshPro>();
			this.Color = new Color(1f, 1f, 1f, 0f);
			Material[] sharedMaterials = this.textMesh.GetComponent<MeshRenderer>().sharedMaterials;
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				sharedMaterials[i].renderQueue = 3610;
			}
		}

		// Token: 0x0600DCCF RID: 56527 RVA: 0x00523208 File Offset: 0x00521408
		public override void WrapAroundPlanetSurface(PlanetLayer layer)
		{
			this.textMesh.ForceMeshUpdate(false, false);
			TMP_TextInfo textInfo = this.textMesh.textInfo;
			int characterCount = textInfo.characterCount;
			if (characterCount == 0)
			{
				return;
			}
			float num = this.textMesh.bounds.extents.x * 2f;
			float num2 = layer.DistOnSurfaceToAngle(num);
			Matrix4x4 localToWorldMatrix = this.textMesh.transform.localToWorldMatrix;
			Matrix4x4 worldToLocalMatrix = this.textMesh.transform.worldToLocalMatrix;
			for (int i = 0; i < characterCount; i++)
			{
				TMP_CharacterInfo tmp_CharacterInfo = textInfo.characterInfo[i];
				if (tmp_CharacterInfo.isVisible)
				{
					int materialReferenceIndex = this.textMesh.textInfo.characterInfo[i].materialReferenceIndex;
					int vertexIndex = tmp_CharacterInfo.vertexIndex;
					Vector3 vector = this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex] + this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 1] + this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 2] + this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 3];
					vector /= 4f;
					float num3 = vector.x / (num / 2f);
					bool flag = num3 >= 0f;
					num3 = Mathf.Abs(num3);
					float num4 = num2 / 2f * num3;
					float num5 = (180f - num4) / 2f;
					float num6 = 200f * Mathf.Tan(num4 / 2f * 0.017453292f);
					Vector3 vector2 = new Vector3(Mathf.Sin(num5 * 0.017453292f) * num6 * (flag ? 1f : -1f), vector.y, Mathf.Cos(num5 * 0.017453292f) * num6);
					Vector3 b = vector2 - vector;
					Vector3 vector3 = this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex] + b;
					Vector3 vector4 = this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 1] + b;
					Vector3 vector5 = this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 2] + b;
					Vector3 vector6 = this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 3] + b;
					Quaternion rotation = Quaternion.Euler(0f, num4 * (flag ? -1f : 1f), 0f);
					vector3 = rotation * (vector3 - vector2) + vector2;
					vector4 = rotation * (vector4 - vector2) + vector2;
					vector5 = rotation * (vector5 - vector2) + vector2;
					vector6 = rotation * (vector6 - vector2) + vector2;
					vector3 = worldToLocalMatrix.MultiplyPoint(localToWorldMatrix.MultiplyPoint(vector3).normalized * (layer.Radius + 0.4f));
					vector4 = worldToLocalMatrix.MultiplyPoint(localToWorldMatrix.MultiplyPoint(vector4).normalized * (layer.Radius + 0.4f));
					vector5 = worldToLocalMatrix.MultiplyPoint(localToWorldMatrix.MultiplyPoint(vector5).normalized * (layer.Radius + 0.4f));
					vector6 = worldToLocalMatrix.MultiplyPoint(localToWorldMatrix.MultiplyPoint(vector6).normalized * (layer.Radius + 0.4f));
					this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex] = vector3;
					this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 1] = vector4;
					this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 2] = vector5;
					this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 3] = vector6;
				}
			}
			this.textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
		}

		// Token: 0x04008D79 RID: 36217
		private TextMeshPro textMesh;

		// Token: 0x04008D7A RID: 36218
		public static readonly GameObject WorldTextPrefab = Resources.Load<GameObject>("Prefabs/WorldText");

		// Token: 0x04008D7B RID: 36219
		[TweakValue("Interface.World", 0f, 5f)]
		private static float TextScale = 1f;
	}
}
