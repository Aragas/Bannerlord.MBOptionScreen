using MCM.Common;

using System.Collections.Generic;
using System.Linq;

namespace MCM.UI.Actions
{
    internal sealed class UndoRedoStack
    {
        private Stack<IAction> UndoStack { get; }
        private Stack<IAction> RedoStack { get; }

        public bool CanUndo => UndoStack.Count > 0;
        public bool CanRedo => RedoStack.Count > 0;
        public bool ChangesMade => UndoStack.Count > 0;

        public UndoRedoStack()
        {
            UndoStack = new Stack<IAction>();
            RedoStack = new Stack<IAction>();
        }

        public bool RefChanged(IRef @ref)
        {
            var stack = UndoStack.Where(s => Equals(s.Context, @ref)).ToList();
            if (stack.Count == 0)
                return false;

            var firstChange = stack.First();
            var lastChange = stack.Last();
            var originalValue = firstChange.Original;
            var currentValue = lastChange.Value;

            if (originalValue is null && currentValue is null)
                return false;
            if (originalValue is null && currentValue is not null)
                return true;
            if (originalValue is not null && currentValue is null)
                return true;
            return !originalValue!.Equals(currentValue!);
        }

        /// <summary>
        /// Adds the action to the UndoStack and calls the Do function.
        /// </summary>
        /// <param name="action"></param>
        public void Do(IAction action)
        {
            action.DoAction();
            UndoStack.Push(action);
            RedoStack.Clear();
        }

        /// <summary>
        /// Calls the Undo function for the top item in the UndoStack. If there is nothing in the stack, does nothing.
        /// </summary>
        public void Undo()
        {
            if (CanUndo)
            {
                var a = UndoStack.Pop();
                a.UndoAction();
                RedoStack.Push(a);
            }
        }

        /// <summary>
        /// Calls the Do function for the top item in the RedoStack. If there is nothing in the stack, does nothing.
        /// </summary>
        public void Redo()
        {
            if (CanRedo)
            {
                var a = RedoStack.Pop();
                a.DoAction();
                UndoStack.Push(a);
            }
        }

        /// <summary>
        /// Calls Undo method for all actions in the UndoStack and the InitialStack, from top to bottom.
        /// </summary>
        public void UndoAll()
        {
            if (CanUndo)
            {
                while (UndoStack.Count > 0)
                {
                    var a = UndoStack.Pop();
                    a.UndoAction();
                }
            }
        }

        public void ClearStack()
        {
            UndoStack.Clear();
            RedoStack.Clear();
        }
    }
}