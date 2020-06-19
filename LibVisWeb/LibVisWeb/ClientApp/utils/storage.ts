// * * * * * * * * * * * 
//
//   Helper para acessar o local storage em typescript
//
//   Feito com base em: https://gist.github.com/Digiman/9fc2640b84bbe5162cf1
//
//
// * * * * * * * * * * * 

//
//  Descrição de item de cookie
//
export interface IStorageItem {

    key: string | null;
    value: any | null;

}

//
//   Item de cookie
//
export class StorageItem {

    key: string;
    value: any;

    constructor(data: IStorageItem) {

        if (data.key != null)
            this.key = data.key;

        this.value = data.value;

    }

}

//
//  Exporta como storage
//
export default class storage {

    // Suporta storage?
    localStorageSupported: boolean;

    //
    //  Singleton
    //
    private static _instance: storage;

    //
    //  Singleton
    //
    private constructor() {

        this.localStorageSupported = typeof window['localStorage'] != "undefined" && window['localStorage'] != null;

    }

    //
    //  Pega o corrente
    //
    public static get cur() {

        return this._instance || (this._instance = new this());

    }

    //
    //  Inclui um valor
    //
    public static add(key: string, item: string) {

        if (this.cur.localStorageSupported) {

            localStorage.setItem(key, item);

        }

    }

    //
    //  Pega todos os itens
    //    
    public static getAllItems(): Array<StorageItem> {

        var list = new Array<StorageItem>();

        for (var i = 0; i < localStorage.length; i++) {

            var key = localStorage.key(i);

            if (key != null) {

                var value = localStorage.getItem(key);

                list.push(new StorageItem({
                    key: key,
                    value: value
                }));

            }

        }

        return list;
    }

    //
    //  Pega todos os valores (só os valores)
    //
    public static getAllValues(): Array<any> {

        var list = new Array<any>();

        for (var i = 0; i < localStorage.length; i++) {

            var key = localStorage.key(i);

            if (key != null) {

                var value = localStorage.getItem(key);
                list.push(value);

            }

        }

        return list;
    }

    //
    //  Pega um item
    //
    public static get(key: string): string | null {

        if (this.cur.localStorageSupported) {

            var item = localStorage.getItem(key);
            return item;

        } else {

            return null;

        }

    }

    //
    //  Remove um item
    //
    public static remove(key: string) {

        if (this.cur.localStorageSupported) {

            localStorage.removeItem(key);

        }

    }

    //
    //  Limpa o storage
    //
    public static clear() {

        if (this.cur.localStorageSupported) {

            localStorage.clear();

        }

    }

}
