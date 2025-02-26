using System;
using Crew.Model.Data;
using Crew.Model.Definitions;
using Crew.Model.Definitions.Localization;
using Crew.UI.HUD.Dialogs;
using UnityEngine;

namespace Crew.Components.Dialogs
{
    public class ShowDialogComponent : MonoBehaviour
    {
        [SerializeField] private Mode _mode;
        [SerializeField] private DialogData _bound;
        [SerializeField] private DialogDef _external;

        private DialogBoxController _dialogBox;

        public void Show()
        {
            _dialogBox = FindDialogController();

            _dialogBox.ShowDialog(Data);
        }
        
        private DialogBoxController FindDialogController()
        {
            if (_dialogBox != null) return _dialogBox;

            GameObject contollerGo;
            switch (Data.Type)
            {
                case DialogType.Simple:
                    contollerGo = GameObject.FindWithTag("SimpleDialog");
                    break;
                case DialogType.Personalized:
                    contollerGo = GameObject.FindWithTag("PersonalDialog");
                    break;
                default:
                    throw new ArgumentException("NoN dialog type");
            }

            return contollerGo.GetComponent<DialogBoxController>();
        }

        public void Show(DialogDef def)
        {
            _external = def;
            Show();
        }

        public DialogData Data
        {
            get
            {
                switch (_mode)
                {
                    case Mode.Bound:
                        return _bound;
                    case Mode.External:
                        return _external.Data;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public enum Mode
        {
            Bound,
            External
        }
    }
}
