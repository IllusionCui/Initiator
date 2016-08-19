using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MTUnity.Utils;

namespace MTUnity.UI {
	public class ScrollRectHelper : MonoBehaviour {
		private ScrollRect _scrollRect;
		public ScrollRect ScrollRect {
			get { 
				if (_scrollRect == null) {
					_scrollRect = GetComponent<ScrollRect> ();

					Debug.Assert (_scrollRect != null, "ScrollRectHelper _scrollRect == null: gameObjectName = " + gameObject.name);
				}
				return _scrollRect;
			}
		}

		public void SetContent(List<RectTransform> items, int num = 1, bool adjustPosition = true) {
			RectTransform itemHolder = ScrollRect.content;
			TransformUtil.RemoveAllChildren (itemHolder);

			if (items == null || items.Count == 0) {
				return;
			}

			// reset itemHolder 
			int pageNum = items.Count / num + (items.Count % num == 0 ? 0 : 1);
			float maxW = 0;
			float maxH = 0;
			List<float> pageMaxW = new List<float> ();
			List<float> pageMaxH = new List<float> ();
			List<float> countW = new List<float> ();
			List<float> countH = new List<float> ();
			for(int i = 0; i < pageNum; i++) {
				pageMaxW.Add (0);
				pageMaxH.Add (0);
				countW.Add (i == 0 ? 0 : countW[i - 1]);
				countH.Add (i == 0 ? 0 : countH[i - 1]);
				for(int j = 0; j < num; j++) {
					int index = i * num + j;
					if (index < items.Count) {
						pageMaxW[i] = Mathf.Max (pageMaxW[i], items[index].rect.width);
						pageMaxH[i] = Mathf.Max (pageMaxH[i], items[index].rect.height);
					}
				}
				maxW = Mathf.Max (maxW, pageMaxW[i]);
				maxH = Mathf.Max (maxH, pageMaxH[i]);
				countW[i] += pageMaxW[i];
				countH[i] += pageMaxH[i];
			}

			if (ScrollRect.horizontal) {
				float posW = adjustPosition ? countW [pageNum - 1] / 2 : itemHolder.localPosition.x;
				itemHolder.anchorMin = new Vector2 (0, 0.5f);
				itemHolder.anchorMax = new Vector2 (0, 0.5f);
				itemHolder.anchoredPosition = Vector3.zero;
				itemHolder.sizeDelta = new Vector2 (countW[pageNum - 1], maxH*num);
				itemHolder.localPosition = new Vector3 (posW, 0, 0);
			} else {
				float posH = adjustPosition ? -countH[pageNum - 1]/2 / 2 : itemHolder.localPosition.y;
				itemHolder.anchorMin = new Vector2 (0.5f, 1);
				itemHolder.anchorMax = new Vector2 (0.5f, 1);
				itemHolder.anchoredPosition = Vector3.zero;
				itemHolder.sizeDelta = new Vector2 (maxW*num, countH[pageNum - 1]);
				itemHolder.localPosition = new Vector3 (0, posH, 0);
			}

			// reset children
			for(int i = 0; i < pageNum; i++) {
				for(int j = 0; j < num; j++) {
					int index = i * num + j;
					if (index < items.Count) {
						RectTransform item = items [index];
						item.SetParent (itemHolder, false);
						if (ScrollRect.horizontal) {
							item.anchorMin = new Vector2 (0, 0.5f);
							item.anchorMax = new Vector2 (0, 0.5f);
							item.anchoredPosition = Vector3.zero;
							item.localPosition = new Vector3 (((i == 0 ? 0 : countW[i - 1]) + countW[i] - countW[pageNum - 1])/2, maxH*(num-1-j*2)/2, 0);
						} else {
							item.anchorMin = new Vector2 (0.5f, 0);
							item.anchorMax = new Vector2 (0.5f, 0);
							item.anchoredPosition = Vector3.zero;
							item.localPosition = new Vector3 (maxW*(j*2-num+1)/2, (((pageNum - 1 - i) == 0 ? 0 : countH[pageNum - 2 - i]) + countH[(pageNum - 1 - i)] - countH[pageNum - 1])/2, 0);
						}
					}
				}
			}
		}
	}
}
