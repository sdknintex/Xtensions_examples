# Gmail custom connector
You can use a serverless function to take parameters from Nintex Workflow Cloud and preprocess them into the format the API requires.
In this example, we use a serverless function to preprocess the email recipients, email subject and message into a format required by the Gmail API, so that we can send a Gmail email from within Nintex Workflow Cloud.

More information on this api is available here: https://developers.google.com/gmail/api/v1/reference/users/messages/send.

We strongly recommend you create a test Google account to test this custom connector, rather than a personal account. For security reasons, Google restricts access to the Gmail API, and you will need to remove these protections on the test account to test your workflow. This does not affect your Google developer account.

To remove the protections: Using the test account, join the Google group at this link: https://groups.google.com/forum/#!forum/risky-access-by-unreviewed-apps  

Joining the group removes the protections that prevent unreviewed apps from accessing the Gmail API on this account. When you have finished testing this example, you can leave this group to restore those protections.

Note: The serverless function in this example is provided on an as-is basis for this demonstration only, and is not suitable for a production environment. Example serverless functions are not covered by the service level agreement.
