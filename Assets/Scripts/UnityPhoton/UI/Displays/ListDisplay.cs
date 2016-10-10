using System;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils.EventUtils;
using UnityUtils.Extensions;
using UObject = UnityEngine.Object;

namespace UnityPhoton {
    public class ListDisplay<ElementData, Element>
        where Element : Component, IListDisplayElement<ElementData>
    {
        private RectTransform elementContainer;
        private UObject elementPrefab;
        private Comparison<ElementData> elementDataComparison;

        public readonly Event<Element> LoadElementDisplayEvent;

        public ListDisplay(
            RectTransform elementContainer,
            UObject elementPrefab,
            Comparison<ElementData> elementDataComparison)
        {
            this.elementContainer = elementContainer;
            this.elementPrefab = elementPrefab;
            this.elementDataComparison = elementDataComparison;
            LoadElementDisplayEvent = new Event<Element>();
        }

        public void Load(IEnumerable<ElementData> elementDatas) {
            var elementDatasList = new List<ElementData>(elementDatas);
            DestroyOldElementDisplays();
            CheckSortElementDatasList(elementDatasList);
            elementDatasList.ForEach(LoadElementDisplay);
        }

        private void CheckSortElementDatasList(List<ElementData> elementDatasList) {
            if (elementDataComparison != null) {
                elementDatasList.Sort(elementDataComparison);
            }
        }

        private void DestroyOldElementDisplays() {
            foreach (Transform element in elementContainer) {
                UObject.Destroy(element.gameObject);
            }
        }

        private void LoadElementDisplay(ElementData elementData) {
            var element = elementContainer.InstantiateChild<Element>(elementPrefab);
            element.Init(elementData);
            LoadElementDisplayEvent.Trigger(element);
        }
    }
}