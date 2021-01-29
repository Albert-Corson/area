import {action, configure, makeAutoObservable, observable, observe} from 'mobx';
import {block} from 'react-native-reanimated';
import Grid from '../Tools/Grid';
import {Block, Size} from '../Types/Block';
import {RootStore} from './RootStore';

const seedData = [
  {color: '#69a58b'},
  {color: '#9b3e63'},
  {color: '#97697d', size: Size.full},
  {color: '#dda022'},
  {color: '#dd5555'},
];

configure({enforceActions: 'always'});

export class GridStore {
    rootStore: RootStore;
    @observable private _modifying = false;
    @observable private _blocks: Array<Block> = [];


    constructor(rootStore: RootStore) {
      makeAutoObservable(this);
      this.rootStore = rootStore;
      this.initGrid();
    }

    @action
    public toggleEditionMode = (): void => {
      this._modifying = !this._modifying;
    }

    public get modifying() {
      return this._modifying;
    }

    private get fillBlock() {
      return ({color: '#00000000', unactive: true});
    }

    @action
    private initGrid = (): void => {
      this._blocks = Grid.fillGrid<Block>(
        seedData,
        this.fillBlock,
        8
      );
    }

    @action
    public get blocks() {
      return this._blocks;
    }

    private addEmptyBlock = (blocks: Block[], index: number): void => {
      blocks.splice(index, 0, this.fillBlock);
    }

    @action
    private removeAt = (blocks: Block[], index: number): void => {
      blocks.splice(index, 1);
    }

    @action
    public swithAtIndexes = (index1: number, index2: number, blocks: Block[] = this._blocks): void => {
      const tmp: Block = blocks[index1];

      blocks[index1] = blocks[index2];
      blocks[index2] = tmp;
    }

    @action
    public setBlockSize = (blockIndex: number, newBlockSize: number) => {
      if (blockIndex < 0 || blockIndex >= this._blocks.length) {
        return;
      }

      const copy: Block[] = [...this._blocks];

      copy[blockIndex].size = newBlockSize;

      for (let i = 0; i < copy.length - 1; ++i) {
        const position: number = Grid.indexToPosition(copy, i);

        if (position % 2 && (copy[i].size ?? Size.normal) !== Size.normal) {
          this.addEmptyBlock(copy, i);
        }
        if (newBlockSize === Size.extended && copy[i + 1].unactive) {
          this.removeAt(copy, i + 1);
          i++;
        }
      }

      this._blocks = copy;
    }

    public getBlockSize = (blockIndex: number): number => {
      return this._blocks[blockIndex]?.size ?? Size.normal;
    }

    public isBlockMutable = (blockIndex: number): boolean => {
      try {
        return this._blocks[blockIndex].unactive ? !this._blocks[blockIndex].unactive : this._modifying;
      } catch {
        return false;
      }
    }
}