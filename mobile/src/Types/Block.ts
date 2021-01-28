export enum Size {
    normal = 1,
    extended = 2,
    full = 4
}

export interface Block {
    size?: Size;
    unactive?: boolean;
}