import type {
    Beatmap,
    DifficultyAttrs,
    DifficultyAttrsData,
    PerformanceAttrsData,
    ScoreInfoData,
    TimedDifficultyAttrsData
} from '../native/dist/binding';

const exports = require('../native/dist/binding.node');

export const Beatmap = exports.Beatmap as Beatmap;
export const DifficultyAttrs = exports.DifficultyAttrs as DifficultyAttrs;
export const GradualDifficulty = exports.GradualDifficulty as GradualDifficulty;
export const DifficultyAttrsData = exports.DifficultyAttrsData as DifficultyAttrsData;
export const PerformanceAttrsData = exports.PerformanceAttrsData as PerformanceAttrsData;
export const ScoreInfoData = exports.ScoreInfoData as ScoreInfoData;
export const TimedDifficultyAttrsData = exports.TimedDifficultyAttrsData as TimedDifficultyAttrsData;