using System;
using RimWorld.Planet;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace Verse
{
	// Token: 0x02000201 RID: 513
	public static class Current
	{
		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000EB7 RID: 3767 RVA: 0x0004BF63 File Offset: 0x0004A163
		public static Root Root
		{
			get
			{
				return Current.rootInt;
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000EB8 RID: 3768 RVA: 0x0004BF6A File Offset: 0x0004A16A
		public static Root_Entry Root_Entry
		{
			get
			{
				return Current.rootEntryInt;
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000EB9 RID: 3769 RVA: 0x0004BF71 File Offset: 0x0004A171
		public static Root_Play Root_Play
		{
			get
			{
				return Current.rootPlayInt;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000EBA RID: 3770 RVA: 0x0004BF78 File Offset: 0x0004A178
		public static Camera Camera
		{
			get
			{
				return Current.cameraInt;
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000EBB RID: 3771 RVA: 0x0004BF7F File Offset: 0x0004A17F
		public static CameraDriver CameraDriver
		{
			get
			{
				return Current.cameraDriverInt;
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000EBC RID: 3772 RVA: 0x0004BF86 File Offset: 0x0004A186
		public static ColorCorrectionCurves ColorCorrectionCurves
		{
			get
			{
				return Current.colorCorrectionCurvesInt;
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000EBD RID: 3773 RVA: 0x0004BF8D File Offset: 0x0004A18D
		public static SubcameraDriver SubcameraDriver
		{
			get
			{
				return Current.subcameraDriverInt;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000EBE RID: 3774 RVA: 0x0004BF94 File Offset: 0x0004A194
		// (set) Token: 0x06000EBF RID: 3775 RVA: 0x0004BF9B File Offset: 0x0004A19B
		public static Game Game
		{
			get
			{
				return Current.gameInt;
			}
			set
			{
				Current.gameInt = value;
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000EC0 RID: 3776 RVA: 0x0004BFA3 File Offset: 0x0004A1A3
		// (set) Token: 0x06000EC1 RID: 3777 RVA: 0x0004BFAA File Offset: 0x0004A1AA
		public static World CreatingWorld
		{
			get
			{
				return Current.creatingWorldInt;
			}
			set
			{
				Current.creatingWorldInt = value;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000EC2 RID: 3778 RVA: 0x0004BFB2 File Offset: 0x0004A1B2
		// (set) Token: 0x06000EC3 RID: 3779 RVA: 0x0004BFB9 File Offset: 0x0004A1B9
		public static ProgramState ProgramState
		{
			get
			{
				return Current.programStateInt;
			}
			set
			{
				Current.programStateInt = value;
			}
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x0004BFC4 File Offset: 0x0004A1C4
		public static void Notify_LoadedSceneChanged()
		{
			Current.cameraInt = GameObject.Find("Camera").GetComponent<Camera>();
			if (GenScene.InEntryScene)
			{
				Current.ProgramState = ProgramState.Entry;
				Current.rootEntryInt = GameObject.Find("GameRoot").GetComponent<Root_Entry>();
				Current.rootPlayInt = null;
				Current.rootInt = Current.rootEntryInt;
				Current.cameraDriverInt = null;
				Current.colorCorrectionCurvesInt = null;
				return;
			}
			if (GenScene.InPlayScene)
			{
				Current.ProgramState = ProgramState.MapInitializing;
				Current.rootEntryInt = null;
				Current.rootPlayInt = GameObject.Find("GameRoot").GetComponent<Root_Play>();
				Current.rootInt = Current.rootPlayInt;
				Current.cameraDriverInt = Current.cameraInt.GetComponent<CameraDriver>();
				Current.colorCorrectionCurvesInt = Current.cameraInt.GetComponent<ColorCorrectionCurves>();
				Current.subcameraDriverInt = GameObject.Find("Subcameras").GetComponent<SubcameraDriver>();
			}
		}

		// Token: 0x04000E6D RID: 3693
		private static ProgramState programStateInt;

		// Token: 0x04000E6E RID: 3694
		private static Root rootInt;

		// Token: 0x04000E6F RID: 3695
		private static Root_Entry rootEntryInt;

		// Token: 0x04000E70 RID: 3696
		private static Root_Play rootPlayInt;

		// Token: 0x04000E71 RID: 3697
		private static Camera cameraInt;

		// Token: 0x04000E72 RID: 3698
		private static CameraDriver cameraDriverInt;

		// Token: 0x04000E73 RID: 3699
		private static ColorCorrectionCurves colorCorrectionCurvesInt;

		// Token: 0x04000E74 RID: 3700
		private static SubcameraDriver subcameraDriverInt;

		// Token: 0x04000E75 RID: 3701
		private static Game gameInt;

		// Token: 0x04000E76 RID: 3702
		private static World creatingWorldInt;
	}
}
