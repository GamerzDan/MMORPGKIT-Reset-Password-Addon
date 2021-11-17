# MMORPGKIT Reset Password Addon
 A custom addon for the MMORPGKIT to add a easy code interface to allow resetting or updating of user password in both MySQL and SQLITE database systems.  
 This addon replicates the kit's own CreateUser database flow, thus so many files even though so little changes or one-liner code.  
 
 You can use this method by using ```MMOClientInstance.Singleton.RequestPasswordReset(Username, Password, OnReset);```  
 OnReset is your callback method  (In my OnReset callback below, I try to re-login immediately using new userid/password).  
 
 ```
 private void OnReset(ResponseHandlerData requestHandler, AckResponseCode responseCode, ResponseUserRegisterMessage response)
        {
            if (responseCode.ShowUnhandledResponseMessageDialog(response.message))
            {
                Debug.Log(response.message);
                if (onLoginFail != null)
                    onLoginFail.Invoke();
                return;
            }
            //Request Login Again
            Debug.Log("Password updated in game backend too");
            PlayerPrefs.SetString(keyUsername, Username);
            MMOClientInstance.Singleton.RequestUserLogin(Username, Password, OnLogin);
        }
 ```
