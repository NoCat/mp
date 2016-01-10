/// <reference path="../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../scripts/typings/bootstrap/bootstrap.d.ts" />

module mp
{
    export interface AjaxResult
    {
        Success: boolean;
        Message: string;
        Data: any;
    }
}