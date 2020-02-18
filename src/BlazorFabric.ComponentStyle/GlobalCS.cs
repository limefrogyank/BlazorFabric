﻿using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public class GlobalCS : ComponentBase, IGlobalCSSheet, IDisposable
    {
        [Inject]
        public IComponentStyle ComponentStyle { get; set; }

        /// <summary>
        /// P
        /// </summary>
        [Parameter]
        public object Component { get; set; }

        /// <summary>
        /// This will garantee that style will stay in header also when all GlobalCS Components related to the Property Component are disposed
        /// </summary>
        [Parameter]
        public bool FixStyle { get; set; } = false;

        /// <summary>
        /// Set to true to reload all Styles
        /// </summary>
        [Parameter]
        public bool ReloadStyle { get; set; } = false;

        /// <summary>
        /// Function which create CSS-Rules which should be printed in head tag of index.html
        /// </summary>
        [Parameter]
        public Func<ICollection<Rule>> CreateGlobalCss { get; set; }

        [Parameter]
        public EventCallback<bool> ReloadStyleChanged { get; set; }

        public bool IsGlobal { get; set; }

        public void Dispose()
        {
            ComponentStyle.GlobalCSSheets.Remove(this);
        }

        protected override Task OnInitializedAsync()
        {
            ComponentStyle.GlobalCSSheets.Add(this);
            return base.OnInitializedAsync();
        }

        protected override void OnParametersSet()
        {
            if (ReloadStyle && IsGlobal)
            {
                ReloadStyle = false;
                ReloadStyleChanged.InvokeAsync(false);
                ComponentStyle.RulesChanged(this);
            }
            base.OnParametersSet();
        }
    }
}

