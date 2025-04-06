namespace E_BangAppEmailBuilder.src.Templates
{
    internal static class DefaultEmailTemplates
    {
        internal static string DefaultHeader(string email)
        {
            return $@"
                        <h1>Welcome {email}!</h1>
                    ";
        }
        internal static string DefaultFullMessage(string header, string body, string footer)
        {
             return $@"
                    <!DOCTYPE html>
                    <html lang=""en"">
                    <head>
                        <meta charset=""UTF-8"">
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                        <title>Default Email Template</title>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                margin: 0;
                                padding: 0;
                                background-color: #f4f4f4;
                            }}
                            .email-container {{
                                max-width: 600px;
                                margin: 20px auto;
                                background: #fff;
                                padding: 20px;
                                border-radius: 5px;
                                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
                            }}
                            .email-header {{
                                text-align: center;
                                margin-bottom: 20px;
                            }}
                            .email-header h1 {{
                                font-size: 24px;
                                color: #333;
                            }}
                            .email-body {{
                                font-size: 16px;
                                color: #666;
                                line-height: 1.6;
                            }}
                            .email-footer {{
                                text-align: center;
                                margin-top: 20px;
                                font-size: 14px;
                                color: #999;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class=""email-container"">
                            <div class=""email-header"">
                                {header}
                            </div>
                            <div class=""email-body"">
                                {body}
                            </div>
                            <div class=""email-footer"">
                                {footer}
                            </div>
                        </div>
                    </body>
                    </html>";
        }
    }
}
