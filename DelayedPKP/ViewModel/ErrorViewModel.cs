using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelayedPKP.ViewModel
{

    /// <summary>
    /// View model of handling every exception that occur in view model.
    /// </summary>
    public class ErrorViewModel : ObservedClass, IViewModel
    {

        #region Constructors

        public ErrorViewModel(IViewModel parent)
        {
            ParentViewModel = parent;
        }

        #endregion

        #region Private Fields

        #endregion

        #region Other view models

        /// <summary>
        /// View model that contains 
        /// </summary>
        public IViewModel ParentViewModel { get; }

        /// <summary>
        /// View model of handling exceptions.
        /// </summary>
        ErrorViewModel IViewModel.ErrorViewModel => this;

        #endregion

        #region Properties

        /// <summary>
        /// Current exception that has been caught.
        /// </summary>
        public Exception CurrentException { get; private set; }

        /// <summary>
        /// Extra message of current exception.
        /// </summary>
        public string ExtraMessage { get; private set; } = string.Empty;

        /// <summary>
        /// Gets true if an error is caught and not confirmed.
        /// </summary>
        public bool IsExceptionCaughtCurrently
        {
            get;
            private set;
        } = false;

        /// <summary>
        /// Event that invokes when a new exception just occurred. 
        /// </summary>
        public event EventHandler ExceptionOccurred;

        /// <summary>
        /// Event that invokes when the exception has been handled.
        /// </summary>
        public event EventHandler ExceptionHanled;

        #endregion

        #region Methods

        /// <summary>
        /// Do a specific action and handle every exception that may occur.
        /// </summary>
        /// <param name="action">Delegate with action to do.</param>
        /// <returns>Returns true if any exception has not been caught, otherwise returns false.</returns>
        public bool DoAndHandleExceptions(Action action)
        {
            try { action(); }
            catch (Exception ex)
            {
                CallException(ex, ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Calls that a new exception occured.
        /// </summary>
        /// <param name="ex">A new exception.</param>
        public void CallException(Exception ex)
        {
            CurrentException = ex;
            ExtraMessage = ex.Message;
            IsExceptionCaughtCurrently = true;

            OnExceptionOccured();
        }

        /// <summary>
        /// Calls that a new exception occured.
        /// </summary>
        /// <param name="ex">A new exception.</param>
        /// <param name="message">Extra message.</param>
        public void CallException(Exception ex, string message)
        {
            CurrentException = ex;
            ExtraMessage = message;
            IsExceptionCaughtCurrently = true;
            
            OnExceptionOccured();
        }

        /// <summary>
        /// Clears current exception.
        /// </summary>
        public void ClearException()
        {
            CurrentException = null;
            ExtraMessage = string.Empty;
            IsExceptionCaughtCurrently = false;

            OnExceptionHandled();
        }

        #endregion

        #region Private methods
        
        /// <summary>
        /// Invokes the ExceptionOccurred event.
        /// </summary>
        protected virtual void OnExceptionOccured()
        {
            ExceptionOccurred?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Invokes the ExceptionHandled event.
        /// </summary>
        protected virtual void OnExceptionHandled()
        {
            ExceptionHanled?.Invoke(this, EventArgs.Empty);
        }

        #endregion

    }
}
