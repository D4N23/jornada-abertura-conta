import type {DeviceContext} from '../../shared/device-context';

export interface DeviceContextRepository {
  loadOrCreate(): DeviceContext;
}