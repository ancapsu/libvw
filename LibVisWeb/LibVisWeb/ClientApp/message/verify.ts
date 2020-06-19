
var validEmailChars: string = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789áéíóúÁÉÍÓÚâêôÂÊÔãõÃÕçÇ@_-.,';
var validCodeChars: string = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';

function isValid(str: string, validChars: string): boolean {

    for (var i: number = 0; i < str.length; i++) {

        if (validChars.indexOf(str[i]) < 0)
            return false;

    }

    return true;

}

function ValidateEmail(email: string): boolean {

    return /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(email);
    
}

export function isValidCode(str: string): boolean { return str.length == 8 && isValid(str, validCodeChars); }
export function isValidSessionCode(str: string): boolean { return str.length < 250 && isValid(str, validCodeChars); }
export function isValidEmail(str: string): boolean { return str.length < 250 && isValid(str, validEmailChars) && ValidateEmail(str); }

