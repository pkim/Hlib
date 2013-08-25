/*
 * Filename: HotKeyManager.cs
 * Author: Lukas Bernreiter, Patrik Kimmeswenger
 * Last change: 21.11.2011
 * 
 * Description: 
 * 
 * This class dedect pressed hotkeys, which are defined in a other class
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Controls;
using System.Reflection;
using System.Reflection.Emit;

using Handler.Item.Reflection;


namespace Handler.Settings.HotKey
{
    /// <summary>
    /// The HotKeyMangaer detects, if a hotkey has been pressed in the given control object 
    /// </summary>
    /// <typeparam name="HotKeyT">Includes all combinations of key which are defined as hotkey</typeparam>
    /// <typeparam name="TEnum">The enum cooperates whith the HotKeyT class, because each hotkey must have hotKeyAttribute where the enum value or hotkey combination is defined</typeparam>
    public class HotKeyHandler<HotKeyT, TEnum>
        where HotKeyT : class
    {

        #region Objects

        /// <summary>
        /// the list which contains all pressed special Keys
        /// </summary>
        private List<Key> pressedKeys = new List<Key>();

        /// <summary>
        /// the last key which was pressed
        /// </summary>
        private Key last_Key = Key.None;

        /// <summary>
        /// The control for which the hotkeyManger should listen for hotkeys
        /// </summary>
        private Control control;

        /// <summary>
        /// The list contains all hotkey combinations from HotKeyT
        /// </summary>
        private List<PropertyInfo> hotKeys;

        /// <summary>
        /// The list contains all names of the hotKeys from HotKeyT
        /// </summary>
        private List<String> hotKeyNames;

        // The instance of the class which contains all the hotKeys
        private HotKeyT hotKeyT;


        #region Constants

        // The maximum count of combinations of hotKeys
        private const Int32 Max_pressedKeys_Count = 4;

        #endregion


        #region Properties

        /// <summary>
        /// Get the name of the HotKeys
        /// </summary>
        public List<String> HotKeyNames
        {
            get
            {
                if (this.hotKeyNames == null)
                    this.hotKeyNames = ReflectionHandler.GetPropertyNames<HotKeyT>();

                return this.hotKeyNames;
            }
        }

        /// <summary>
        /// The Char which connects the key in the hotkey strings
        /// </summary>
        private Char ConnectorChar
        { get { return '+'; } }

        /// <summary>
        /// The last pressed key
        /// </summary>
        public Key Last
        { get { return last_Key; } }

        #endregion


        #endregion


        #region Delegates

        // This handler serves the enum value of the pressed hotKey.
        public delegate void HotKeyPressedHandler(TEnum _hotKeyName);

        #endregion


        #region Events Definition

        // This event will accure if a the hotKeyManger has dedected a HotKey
        public event HotKeyPressedHandler HotKeyPressed;

        #endregion


        #region Constructor

        /// <summary>
        /// The HotKeyMangaer detects, if a hotkey has been pressed in the given control object
        /// </summary>
        /// <param name="_control">The control on which the hotkeyManger should listen for hotkeys</param>
        public HotKeyHandler(Control _control)
        {
            // set the control
            this.control = _control;

            // initialze the eventhandler, which listens on key down events
            this.control.PreviewKeyDown += this.KeyDownEvent;

            // get all hotKey combinations
            this.hotKeys = ReflectionHandler.GetProperties<HotKeyT>();

            // get a instance of the hotKey class which contains all defined hotKeys
            this.hotKeyT = ReflectionHandler.GetInstance<HotKeyT>();
        }

        /// <summary>
        /// The HotKeyMangaer detects, if a hotkey has been pressed in the given control object
        /// </summary>
        /// <param name="_control">The control on which the hotkeyManger should listen for hotkeys</param>
        /// <param name="_hotKeyT">The class which contains all the hotKeys and hotKeyAttributes</param>
        public HotKeyHandler(Control _control, HotKeyT _hotKeyT)
        {
            // set the control
            this.control = _control;

            // initialze the eventhandler, which listens on key down events
            this.control.PreviewKeyDown += this.KeyDownEvent;
            this.control.PreviewKeyUp   += this.KeyUpEvent;

            // get all hotKey combinations
            this.hotKeys = ReflectionHandler.GetProperties<HotKeyT>();

            // get a instance of the hotKey class which contains where all hotKeys are defined
            this.hotKeyT = _hotKeyT;

        }

        #endregion


        #region Events

        private void KeyUpEvent(Object _sender, KeyEventArgs _e)
        {
            TEnum hotKey;
            Key key = _e.Key;

            // if the list of current pressed keys contains the the released key
            if (this.pressedKeys.Contains(key))
            {
                // if a hotKey has been pressed
                if (this.get_Event(_e.Key, out hotKey))
                {
                    // invoke the hotKeyPressed event
                    this.HotKeyPressed(hotKey);
                }

                // remove that key
                this.pressedKeys.Remove(key);
            }
        }

        /// <summary>
        /// The event accures if a key was pressed in the given control object
        /// </summary>
        /// <param name="_sender">The control which sends the hotkey</param>
        /// <param name="_e">the keyEventArguments</param>
        private void KeyDownEvent(Object _sender, KeyEventArgs _e)
        {
            
            Key key = _e.Key;

            // if the pressed key isn't already pressed (impossible, but always keep save)
            if (!this.pressedKeys.Contains(key))
            {
                // add the pressed key to the other currently pressed keys
                pressedKeys.Add(key);

                // set the last pressed key
                this.last_Key = key;


                // while more than the maximum of permanent pressed keys are pressed
                while (this.pressedKeys.Count >= Max_pressedKeys_Count)
                {
                    // remove the first pressed key
                    this.pressedKeys.RemoveAt(0);
                }
            }
        }

        #endregion Events


        #region Methods

        /// <summary>
        /// Dedects if an hotKey has been pressed an which hotKey has been pressed
        /// </summary>
        /// <param name="_key"></param>
        /// <param name="_hotKey">The pressed hotKey</param>
        /// <returns>Returns true if an hotKey has been pressed otherwise false</returns>
        public Boolean get_Event(Key _key, out TEnum _hotKey)
        {
            // set the _hotKey to the default HoKey None
            _hotKey = (TEnum)((HotKeyAttribute)this.hotKeys[0].GetCustomAttributes(typeof(HotKeyAttribute), true)[0]).HotKey;
            
            // if the class which contains all hotKeys isn't null
            if (this.hotKeyT != null)
            {
                // for each hotkey in the list of hotKeys
                foreach (PropertyInfo hotKey in this.hotKeys)
                {
                    // get the value of the current hotKey to check
                    String hotKeyValue = hotKey.GetValue(this.hotKeyT, null) as String;

                    // if the current combination of keys are a hotKey
                    if (this.is_HotKeyEvent(_key, hotKeyValue))
                    {
                        // initialize a hotKeyAttribute which contains the enum value of the hotKey
                        HotKeyAttribute hotKeyAttribute = ReflectionHandler.GetCustomAttribute<HotKeyAttribute>(hotKey, true);

                        if (hotKeyAttribute == null || (TEnum)hotKeyAttribute.HotKey == null)
                            return false;

                        // get the enum value of the hotKey
                        _hotKey = (TEnum)hotKeyAttribute.HotKey;

                        // return true, means a hotKey has been pressed
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Specifies if the pressed key is a HotKeyName
        /// </summary>
        /// <param name="_key">the pressed Key</param>
        /// <param name="_hotKey">the to comparing HotKeyNames</param>
        /// <returns>returns ture if the pressed Key is a HotKeyNames</returns>
        public Boolean is_HotKeyEvent(Key _key,  String _hotKey)
        {
            // if the hotkey isn't initialized
            if (_hotKey == null)
                return false;

            List<String> needed_Keys = this.get_needed_Keys(_hotKey);

            // if unequal
            if (pressedKeys.Count != needed_Keys.Count)
                return false;

            // check sequenze of pressed keys
            for (Int32 i = new Int32(); i < needed_Keys.Count; i++)
            {
                if (!pressedKeys[i].ToString().Equals(needed_Keys[i]))
                    return false;
            }

            return true;
        }


        /// <summary>
        /// Takes the HotKeyEvent and returns the needed keys
        /// </summary>
        /// <returns>Returns the Keys which are neede for an HotKeyEvent</returns>
        private List<String> get_needed_Keys(String _hotKey_Event)
        {
            // The list will contain the needed Keys for the hotKey
            return _hotKey_Event.Split(this.ConnectorChar).ToList();
        }

        #endregion Methods

    }
}
